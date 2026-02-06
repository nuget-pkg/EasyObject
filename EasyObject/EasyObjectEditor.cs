using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Global
{
    using static EasyObject;
    internal static class EasyObjectEditor
    {
        public static EasyObject Clone(
            EasyObject x,
            uint maxDepth = 0,
            List<string> hideKeys = null,
            bool always = true
            )
        {
            if (x == null) return null;
            hideKeys = hideKeys ?? new List<string>();
            if (!always)
            {
                if (maxDepth == 0 && hideKeys.Count == 0)
                {
                    return x;
                }
            }
            x = FromObject( x );
            Trim(x, maxDepth, hideKeys);
            return x;
        }
        public static void Trim(
            EasyObject x,
            uint maxDepth = 0,
            List<string> hideKeys = null
            )
        {
            if (x == null) return;
            hideKeys = hideKeys ?? new List<string>();
            TrimHelper(1, x, maxDepth, hideKeys);
        }
        private static void TrimHelper(
            uint depth,
            EasyObject x,
            uint maxDepth,
            List<string> hideKeys
            )
        {
            if (x == null) return;
            if (maxDepth > 0)
            {
                if (depth >= maxDepth)
                {
                    if (x.IsArray)
                    {
                        for (int i = 0; i < x.Count; i++)
                        {
                            Clear(x.list[i]);
                        }
                        //return;
                    }
                    else if (x.IsObject)
                    {
                        var keys = x.Keys;
                        for (int i = 0; i < keys.Count; i++)
                        {
                            string key = keys[i];
                            Clear(x.dictionary[key]);
                        }
                        //return;
                    }
                }
            }
            if (x.IsArray)
            {
                for (int i = 0; i < x.Count; i++)
                {
                    TrimHelper(depth + 1, x.list[i], maxDepth, hideKeys);
                }
                //return;
            }
            else if (x.IsObject)
            {
                var keys = x.Keys;
                for (int i = 0; i < keys.Count; i++)
                {
                    string key = keys[i];
                    if (hideKeys.Contains(key))
                    {
                        x.dictionary.Remove(key);
                        continue;
                    }
                    TrimHelper(depth + 1, x.dictionary[key], maxDepth, hideKeys);
                }
                //return;
            }
        }

        private static void Clear(EasyObject x)
        {
            if (x == null) return;
            if (x.IsArray)
            {
                x.list.Clear();
            }
            else if (x.IsObject)
            {
                x.dictionary.Clear();
            }
        }

    }
}

