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
            hideKeys = hideKeys ?? new List<string>();
            if (!always) {
                if (maxDepth == 0 && maxCount == 0 && hideKeys.Count == 0) {
                    return x;
                }
            }
            x = FromObject(x);
            Trim(x, maxDepth: maxDepth, maxCount: maxCount, hideKeys: hideKeys);
            return x;
        }
        public static void Trim(
            EasyObject x,
            uint maxDepth = 0,
            uint maxCount = 0,
            List<string>? hideKeys = null
        ) {
            //if (x == null) return;
            hideKeys = (hideKeys ?? new List<string>());
            if (maxCount > 0)
            {
                TrimHelper(1, x, hideKeys, maxDepth: 0, maxCount: maxCount);
            }
            TrimHelper(1, x, hideKeys, maxDepth: maxDepth, maxCount: 0);
        }
        private static EasyObject TrimHelper(
            uint depth,
            EasyObject x,
            List<string> hideKeys,
            uint maxDepth,
            uint maxCount = 0
        ) {
            //if (x == null) return;
            if (maxCount > 0)
            {
                if (x.IsArray)
                {
                    var newList = x.RealList!.Take((int)maxCount).ToList();
                    Console.WriteLine($"newList.Count={newList.Count}");
                    newList = newList.Select(x => TrimHelper(depth + 1, x, hideKeys, maxDepth: maxDepth, maxCount: maxCount)).ToList();
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
                            newDict[keys[i]] = TrimHelper(depth + 1,dict[keys[i]], hideKeys, maxDepth: maxDepth, maxCount: maxCount);
                        }
                        x.RealData = newDict;
                    }
                }
            }
            if (maxDepth > 0) {
                if (depth >= maxDepth) {
                    if (x.IsArray) {
                        for (int i = 0; i < x.Count; i++) {
                            Clear(x.RealList![i]);
                        }
                        //return;
                    }
                    else if (x.IsObject) {
                        var keys = x.Keys;
                        for (int i = 0; i < keys.Count; i++) {
                            string key = keys[i];
                            Clear(x.RealDictionary![key]);
                        }
                    }
                }
            }
            //if (x.IsArray) {
            //    //for (int i = 0; i < x.Count; i++) {
            //    //    TrimHelper(depth + 1, x.RealList![i], maxDepth: maxDepth, maxCount: maxCount, hideKeys: hideKeys);
            //    //}
            //    var newList = x.RealList!.Select(x => TrimHelper(depth + 1, x, maxDepth: maxDepth, maxCount: maxCount)).ToList();
            //    return EasyObject.FromObject(newList);
            //}
            //else if (x.IsObject) {

            //    var keys = x.Keys;
            //    for (int i = 0; i < keys.Count; i++) {
            //        string key = keys[i];
            //        if (hideKeys.Contains(key)) {
            //            x.RealDictionary!.Remove(key);
            //            continue;
            //        }
            //        TrimHelper(depth + 1, x.RealDictionary![key], maxDepth: maxDepth, maxCount: maxCount, hideKeys: hideKeys);
            //    }
            //}
            return EasyObject.FromObject(x);
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