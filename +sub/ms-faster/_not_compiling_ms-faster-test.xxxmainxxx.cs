//css_nuget Microsoft.FASTER.Core
//css_nuget MiniProfiler.Shared
//css_nuget EasyObject
using FASTER.core;
using System.Text;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using StackExchange.Profiling;
using Microsoft.Extensions.DependencyInjection;

using static Global.EasyObject;
using static Global.OpenSystem;

namespace litedbtest
{
    class CompletionContext<T>
    {
        public T Value;
        public bool Completed;
    }
    class Funcs : FunctionsBase<long, long, long, long, CompletionContext<long>>
    {
        // if data is in disk, *Reader and ReadCompletionCallback should be implemented
        public override void SingleReader(ref long key, ref long input, ref long value, ref long dst)
        {
            dst = value;
        }
        public override void ConcurrentReader(ref long key, ref long input, ref long value, ref long dst)
        {
            dst = value;
        }
        public override void ReadCompletionCallback(ref long key, ref long input, ref long output, CompletionContext<long> ctx, Status status)
        {
            ctx.Value = output;
            output = ctx.Value;
            ctx.Completed = true;
        }
    }
    class FasterTest
    {
        class MyEqualityComparer : IFasterEqualityComparer<long>
        {
            public bool Equals(ref long k1, ref long k2)
            {
                return k1 == k2;
            }

            public long GetHashCode64(ref long k)
            {
                return k;
            }
        }
        static string GetTimingTreeString(Timing t, int depth)
        {
            var sb = new StringBuilder();
            var indent = new string(' ', depth * 2);
            if (depth > 100)
            {
                throw new Exception($"too deep({depth})");
            }
            if (t.HasChildren)
            {
                sb.AppendLine($"{indent}{t.Name}: {t.DurationMilliseconds},{t.DurationWithoutChildrenMilliseconds}");
            }
            else
            {
                sb.AppendLine($"{indent}{t.Name}: {t.DurationMilliseconds}");
            }
            if (t.HasChildren)
            {
                foreach (var child in t.Children)
                {
                    sb.Append(GetTimingTreeString(child, depth + 1));
                }
            }
            return sb.ToString();
        }
        public static void TestContinue(int memoryBits, int pageBits, int segmentBits)
        {
            const string checkpointDir = "checkpoints";
            string logPath = Path.Combine("fasterlog", "faster.log");
            if (Directory.Exists(logPath))
            {
                Directory.Delete(logPath);
            }
            Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            if (Directory.Exists(checkpointDir))
            {
                Directory.Delete(checkpointDir, true);
            }
            var logDevice = Devices.CreateLogDevice(logPath, deleteOnClose: true);
            try
            {
                var logSettings = new LogSettings()
                {
                    LogDevice = logDevice,
                    MemorySizeBits = memoryBits,
                    PageSizeBits = pageBits,
                    SegmentSizeBits = segmentBits
                };
                var checkPointSetting = new CheckpointSettings()
                {
                    CheckPointType = CheckpointType.Snapshot,
                    CheckpointDir = checkpointDir
                };
                var faster = new FasterKV<long, long, long, long, CompletionContext<long>, Funcs>(1 << 15,
                    new Funcs(), logSettings, checkpointSettings: checkPointSetting, comparer: new MyEqualityComparer());
                Guid session;
                Guid checkpoint;
                var profiler = MiniProfiler.StartNew($"faster_{memoryBits}_{pageBits}_{segmentBits}");
                using (profiler.Step("insert"))
                {
                    session = faster.StartSession();
                    var ctx = new CompletionContext<long>();
                    using (profiler.Step("upsert"))
                    {
                        for (int loop = 0; loop < 10; loop++)
                        {
                            for (int i = 0; i < 10000; i++)
                            {
                                long key = i;
                                long value = i * 3 + loop * 1000;
                                var st = faster.Upsert(ref key, ref value, ctx, loop * 10000 + i);
                            }
                        }
                    }
                    using (profiler.Step("compaction"))
                    {
                        faster.Log.Compact(faster.Log.TailAddress);
                    }
                    using (profiler.Step("checkpoint"))
                    {
                        faster.TakeFullCheckpoint(out checkpoint);
                        faster.CompleteCheckpoint(true);
                    }
                    faster.StopSession();
                }
                using (profiler.Step("continue"))
                {
                    using (profiler.Step("checkpoint"))
                    {
                        faster.Recover(checkpoint);
                    }
                    var monotonic = faster.ContinueSession(session);
                    faster.StopSession();
                }
                profiler.Stop();
                Console.WriteLine(GetTimingTreeString(profiler.Root, 0));
                faster.Dispose();
            }
            finally
            {
                logDevice.Close();
            }
        }
        public static void TestBasic()
        {
            foreach (var fi in new DirectoryInfo(".").GetFiles("faster.log*"))
            {
                fi.Delete();
            }
            var logDevice = Devices.CreateLogDevice("./faster.log");
            var checkpointSetting = new CheckpointSettings()
            {
                CheckpointDir = "checkpoints"
            };
            var readCacheSettings = new ReadCacheSettings()
            {
                MemorySizeBits = 20,
                PageSizeBits = 8,
            };
            var logSettings = new LogSettings()
            {
                LogDevice = logDevice,
                MemorySizeBits = 20,
                PageSizeBits = 8,
                SegmentSizeBits = 20,
                ReadCacheSettings = readCacheSettings
            };
            using (var faster = new FasterKV<long, long, long, long, CompletionContext<long>, Funcs>(1 << 20,
                new Funcs(),
                logSettings,
                checkpointSettings: checkpointSetting))
            {
                var guid = faster.StartSession();
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                var ctx = new CompletionContext<long>();
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < 100000; j++)
                    {
                        var key = (long)j;
                        var value = (long)(i * 10000 + j);
                        var st = faster.Upsert(ref key, ref value, ctx, 0);
                        if (st != Status.OK)
                        {
                            Console.WriteLine($"upserting failed,{st},{key},{value},{i}");
                        }
                    }
                }
                sw.Stop();
                using (var iter = faster.Log.Scan(faster.Log.BeginAddress, faster.Log.TailAddress))
                {
                    while (iter.GetNext(out var recordInfo, out var key, out var value))
                    {
                    }
                }
                Console.WriteLine($"{sw.Elapsed}");
                for (int i = 0; i < 100; i++)
                {
                    long key = i % 10;
                    long value = 0;
                    long input = 0;
                    var st = faster.Read(ref key, ref input, ref value, ctx, 0);
                    if (st == Status.OK)
                    {
                        // Console.WriteLine($"OK: {key},{input},{value}");
                    }
                    else if (st == Status.PENDING)
                    {
                        faster.CompletePending(true);
                        // Console.WriteLine($"Pending: {key},{input},{value},{ctx.Value},{ctx.Completed}");
                        ctx.Completed = false;
                    }
                }
                Console.WriteLine($"{faster.DumpDistribution()}");
                faster.Log.Compact(faster.Log.TailAddress);
                faster.TakeHybridLogCheckpoint(out var token);
                faster.CompleteCheckpoint(true);
                Console.WriteLine($"checkpoint id: {token}, session id: {guid}");
                faster.StopSession();
            }
            Console.WriteLine($"{logDevice.FileName}");
            logDevice.Close();
        }
    }
}
