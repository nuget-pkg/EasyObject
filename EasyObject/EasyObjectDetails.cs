#if MINIMAL
using EasyObject = Global.MiniEasyObject;
#endif

using System.Collections.Generic;
namespace Global;
#if MINIMAL
internal class MiniEasyObjectConverter : IConvertParsedResult
#else
internal class EasyObjectConverter : IConvertParsedResult
#endif
{
    public object? ConvertParsedResult(object? x, string origTypeName) {
        if (x is Dictionary<string, object> dict) {
            var keys = dict.Keys;
            var result = new Dictionary<string, EasyObject>();
            foreach (var key in keys) {
                var eo = new EasyObject {
                    RealData = dict[key]
                };
                result[key] = eo;
            }
            return result;
        }
        if (x is List<object> list) {
            var result = new List<EasyObject>();
            foreach (var e in list) {
                var eo = new EasyObject {
                    RealData = e
                };
                result.Add(eo);
            }
            return result;
        }
        return x;
    }
}