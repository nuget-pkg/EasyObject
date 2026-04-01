using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using StellarSdk.Exceptions;
using StellarSdk.Model;

namespace StellarSdk
{
    public class CallBuilder
    {
        private String url;
        private List<string> filters;
        private List<KeyValuePair<string, string>> urlParams;
        private List<KeyValuePair<string, string>> bodyParams;
        private String details;

        // services marked as idempotent will be retried in case of transient failures
        protected Boolean isIdempotent;

        public CallBuilder(String serverUrl)
        {
            url = serverUrl;
            filters = new List<string>();
            urlParams = new List<KeyValuePair<string, string>>();
            bodyParams = new List<KeyValuePair<string, string>>();
            isIdempotent = false;
        }

        public void addSegment(string segment)
        {
			url += url.EndsWith("/", StringComparison.Ordinal) ? segment : "/" + segment;
        }

        public void addFilter(string value)
        {
            filters.Add(value);
        }

        public void addDetails(string d)
        {
            details = d;
        }

        public void addParam(string param, string value)
        {
            urlParams.Add(new KeyValuePair<string, string>(param, value));
        }

        public void addBody(string param, string value)
        {
            bodyParams.Add(new KeyValuePair<string, string>(param, value));
        }

        private void checkFilter()
        {
            if(filters.Count >= 2)
            {
                throw new ArgumentException("Invalid filters");
            }
            if(filters.Count == 1)
            {
                url = url + "/" + filters[0];
            }
            if(details != null)
            {
                url = url + "/" + details;
            }
            for (int i = 0; i < urlParams.Count; i++)
            {
                url = url + (i == 0 ? "?" : "&");
                url = url + urlParams[i].Key + "=" + urlParams[i].Value;
            }
        }
        
        public async Task<String> DoCall()
        {
            this.checkFilter();
            int retries = isIdempotent ? 1 : 0;
            for (int attempt = 0; attempt <= retries; attempt++)
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = null;
                    try
                    {
                        if (bodyParams.Count > 0)
                        {
                            // POST
                            var formUrlEncodedContent = new FormUrlEncodedContent(bodyParams);
                            response = await client.PostAsync(url, formUrlEncodedContent);
                        }
                        else
                        {
                            // GET
                            response = await client.GetAsync(url);
                        }
                    }
                    catch (Exception e)
                    {
                        if (attempt < retries)
                        {
                            // retry
                            continue;
                        }
                        else
                        {
                            throw e;
                        }
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        // OK
						var content = await response.Content.ReadAsStringAsync();
						return content;
                    }
                    else if (((int)response.StatusCode) == 404)
                    {
                        // not found
                        throw new ResourceNotFoundException();
                    }
                    else if (((int)response.StatusCode) == 400)
                    {
                        // business error (bad request)
                        string txt = await response.Content.ReadAsStringAsync();
                        BadRequestError err = BadRequestError.FromJson(txt);
                        throw new BadRequestException("Bad request", err);
                    }
                    else if (((int)response.StatusCode) == 500 && attempt < retries)
                    {
                        // technical error, retry
                        continue;
                    }
                    else
                    {
                        throw new TechnicalException("HTTP Error from REST API: " + response.StatusCode);
                    }
                }
            }

            // unreachable
            return null;
        }
    }
}
