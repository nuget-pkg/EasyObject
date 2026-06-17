// ReSharper disable RedundantNameQualifier
// ReSharper disable ForCanBeConvertedToForeach
namespace Global {
    using System;
#if MINIMAL
    using EasyObject = Global.MiniEasyObject;
#endif
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
#if MINIMAL
    using static MiniEasyObject;
#else
    using static EasyObject;
#endif
#if MINIMAL
    internal static class MiniEasyObjectEditor {
#else
    internal static class EasyObjectEditor {
#endif
        public static EasyObject Clone(
            EasyObject x,
            uint maxDepth = 0,
            uint maxCount = 0,
            List<string>? hideKeys = null,
            bool always = true
        ) {
            //if (x == null) return Null;
            //Debug(new {x, maxDepth, maxCount, hideKeys }, "At beginning of Clone()");
            hideKeys = hideKeys ?? new List<string>();
            if (!always) {
                if (maxDepth == 0 && maxCount == 0 && hideKeys.Count == 0) {
                    return x;
                }
            }
            x = FromObject(x);
            //Debug(x, "before Trim()");
            Trim(x, maxDepth: maxDepth, maxCount: maxCount, hideKeys: hideKeys);
            //Debug(x, "after Trim()");
            return x; /**/
        }
        public static void Trim(
            EasyObject x,
            uint maxDepth = 0,
            uint maxCount = 0,
            List<string>? hideKeys = null
        ) {
            //if (x == null) return;
            hideKeys = (hideKeys ?? new List<string>());
            x = TrimCount(x, hideKeys, maxCount: maxCount);
            x = TrimDepth(1, x, hideKeys, maxDepth: maxDepth);
        }
        private static EasyObject TrimDepth(
            uint depth,
            EasyObject x,
            List<string> hideKeys,
            uint maxDepth
        )
        {
            if (maxDepth > 0)
            {
                if (depth >= maxDepth)
                {
                    if (x.IsArray)
                    {
                        for (int i = 0; i < x.Count; i++)
                        {
                            Clear(x.RealList![i]);
                        }
                        //return;
                    }
                    else if (x.IsObject)
                    {
                        var keys = x.Keys;
                        for (int i = 0; i < keys.Count; i++)
                        {
                            string key = keys[i];
                            Clear(x.RealDictionary![key]);
                        }
                    }
                }
            }
            return EasyObject.FromObject(x);
        }
        private static EasyObject TrimCount(
            EasyObject x,
            List<string> hideKeys,
            uint maxCount
        )
        {
            if (maxCount > 0)
            {
                if (x.IsArray)
                {
                    var newList = x.RealList!.Take((int)maxCount).ToList();
                    Console.WriteLine($"newList.Count={newList.Count}");
                    newList = newList.Select(x => TrimCount(x, hideKeys, maxCount: maxCount)).ToList();
                    x.RealData = newList;
                }
                else if (x.IsObject)
                {
                    Dictionary<string, EasyObject> dict = x.RealDictionary!;
                    if (dict.Count > maxCount)
                    {
                        var keys = dict.Keys.Take((int)maxCount).ToList(); ;
                        Dictionary<string, EasyObject> newDict = new Dictionary<string, EasyObject>();
                        for (int i = 0; i < keys.Count; i++)
                        {
                            newDict[keys[i]] = TrimCount(dict[keys[i]], hideKeys, maxCount: maxCount);
                        }
                        x.RealData = newDict;
                    }
                }
            }
            return EasyObject.FromObject(x);
        }
        public static EasyObject ShallowTake(EasyObject x, int n)
        {
            if (x.RealList != null)
            {
                var result = x.RealList!.Select(i => i).Take(n).ToList();
                return FromObject(result);
            }
            else if (x.RealDictionary != null)
            {
                var keys = x.RealDictionary.Keys.Select(i => i).Take(n).ToList();
                var result = NewObject();
                foreach (var key in keys) result[key] = x.RealDictionary[key];
                return result;
            }
            return Clone(x);
        }
        public static EasyObject DeepTake(EasyObject x, int n)
        {
            if (x.RealList != null)
            {
                var result = x.RealList!.Select(i => i).Take(n).ToList();
                result = result.Select(i => DeepTake(i, n)).ToList();
                return FromObject(result);
            }
            else if (x.RealDictionary != null)
            {
                var keys = x.RealDictionary.Keys.Select(i => i).Take(n).ToList();
                var result = NewObject();
                foreach (var key in keys) result[key] = DeepTake(x.RealDictionary[key], n);
                return result;
            }
            return Clone(x);
        }
        private static void Clear(EasyObject x) {
            // (x == null) return;
            if (x.IsArray) {
                x.RealList!.Clear();
            }
            else if (x.IsObject) {
                x.RealDictionary!.Clear();
            }
        }
        public static dynamic? ExportToExpandoObject(EasyObject x) {
            if (x.IsNull) return null;
            if (x.IsArray) {
                var result = new List<dynamic?>();
                var list = x.RealList!;
                foreach (var item in list) {
                    result.Add(ExportToExpandoObject(item));
                }
                return result;
            }
            else if (x.IsObject) {
                var result = new ExpandoObject();
                var dictionary = x.RealDictionary!;
                var keys = dictionary.Keys;
                foreach (var key in keys) {
                    (result as IDictionary<string, object?>)[key] = ExportToExpandoObject(dictionary[key]);
                }
                return result;
            }
            return x.RealData;
        }
    }
}