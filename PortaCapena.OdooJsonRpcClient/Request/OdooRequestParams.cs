using Newtonsoft.Json;
using System;

namespace PortaCapena.OdooJsonRpcClient.Request
{
    public class OdooRequestParams
    {
        [JsonProperty("service")]
        public string Service { get; }

        [JsonProperty("method")]
        public string Method { get; }

        [JsonProperty("args")]
        public object[] Args { get; }

      
        [JsonIgnore]
        public string Url { get; }

        public OdooRequestParams(string url, string service, string method, params object[] parameters)
        {
            Url = url;
            Service = service;
            Method = method;
            Args = RemoveTrailingNulls(parameters);
        }

        /** <summary>
         * Removes all trailing null values from the end of the parameter array and returns the cleaned array.
         * </summary>
         */
        protected static object[] RemoveTrailingNulls(object[] parameters)
        {
            //if null or empty, return empty array
            if (parameters == null || parameters.Length == 0)
                return Array.Empty<object>();

            //get new array length without trailing nulls
            var length = parameters.Length;
            while (length > 0 && parameters[length - 1] == null)
                length--;
            
            //if all nulls, return empty array
            if (length == 0)
                return Array.Empty<object>();
            
            //if no trailing nulls, return original array
            if (length == parameters.Length)
                return parameters;
            
            //otherwise allocate new array and copy values
            var result = new object[length];
            Array.Copy(parameters, result, length);
            return result;
        }
    }
}
