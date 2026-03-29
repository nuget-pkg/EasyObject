namespace Global;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static Global.EasyObject;
public static partial class EasySystemBeta {
    public static string GetRidirectUrl(string url) {
        Task<string> task = GetRidirectUrlTask(url);
        task.Wait();
        return task.Result;
        // !! LOCAL FUNCTIONS BELLOW !!
        async Task<string> GetRidirectUrlTask(string url) {
            HttpClient client;
            HttpResponseMessage response;
            try {
                client = new HttpClient();
                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception) {
                return url;
            }
            string result = response.RequestMessage!.RequestUri!.ToString();
            response.Dispose();
            return result;
        }
    }
}