using System;

namespace PortaCapena.OdooJsonRpcClient.Models
{
    public class OdooConfig
    {
        public string ApiUrl { get; }
        public string ApiUrlJson => ApiUrl + "/jsonrpc";
        public string DbName { get; }
        public string UserName { get; }
        public string Password { get; }
        public TimeSpan Timeout { get; }

        public OdooContext Context { get; }


        public OdooConfig(string apiUrl, string dbName, string userName, string password, TimeSpan timeout = default)
        {
            ApiUrl = apiUrl.TrimEnd(new[] { '/' });
            DbName = dbName;
            UserName = userName;
            Password = password;
            Context = new OdooContext();
            Timeout = timeout;
        }

        public OdooConfig(string apiUrl, string dbName, string userName, string password, OdooContext context, TimeSpan timeout = default) : this(apiUrl, dbName, userName, password, timeout)
        {
            Context = new OdooContext(context);
        }

        public OdooConfig(string apiUrl, string dbName, string userName, string password, string language, TimeSpan timeout = default) : this(apiUrl, dbName, userName, password, timeout)
        {
            Context = new OdooContext(language);
        }

        public OdooConfig(string apiUrl, string dbName, string userName, string password, string language, string timezone, TimeSpan timeout = default) : this(apiUrl, dbName, userName, password, timeout)
        {
            Context = new OdooContext(language, timezone);
        }
    }
}