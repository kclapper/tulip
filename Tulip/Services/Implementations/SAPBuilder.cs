using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

using Tulip.Services.Interfaces;

namespace Tulip.Services.Implementations
{
    class SAPBuilder: ISAPBuilder
    {
        private ILogger<SAP> logger;
        private HttpClient client;
        public SAPBuilder(ILogger<SAP> logger, HttpClient client)
        {
            this.logger = logger;
            this.client = client;

            var sapUserName = "TEACH-003";
            var sapPassword = "Naqiya99"; 
            var authToken = Encoding.ASCII.GetBytes($"{sapUserName}:{sapPassword}");

            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(authToken)
            );
        }

        private string username;
        private string caseStudy;
        private int clientId = -1;
        private string applicationServer;

        public ISAPBuilder SetUsername(string username)
        {
            this.username = username;
            return this;
        }
        public ISAPBuilder SetCaseStudy(string caseStudy)
        {
            this.caseStudy = caseStudy;
            return this;
        }
        public ISAPBuilder SetClientId(int clientId)
        {
            this.clientId = clientId;
            return this;
        }
        public ISAPBuilder SetApplicationServer(string server)
        {
            this.applicationServer = server;
            return this;
        }
        private bool readyToQuery()
        {
            return username != null 
                && caseStudy != null 
                && clientId != -1
                && applicationServer != null;
        }
        public async Task<ISAP> Build() 
        {
            if (!readyToQuery())
            {
                throw new Exception("Not ready to connect to SAP");
            }

            string url = getConnectionString();
            logger.LogInformation($"SAP URL: {url}");

            HttpResponseMessage result;
            try 
            {
                result = await client.GetAsync(url);
                logger.LogInformation($"SAP Response Code: {result.StatusCode}");

                if ((int) result.StatusCode >= 400)
                {
                    logger.LogError($"Bad SAP response code: {result.StatusCode}");
                    throw new SAPException($"Could not connect to SAP, status code: {result.StatusCode}");
                }

                var body = await getBodyContent(result);

                return new SAP(logger, body);
            } 
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw new SAPException("SAPBuilder encountered an error while connecting to SAP", e);
            }
       }

        private string getConnectionString()
        {
            return $"http://{applicationServer.Trim()}/sap/opu/odata/sap/ZUCC_GBM_GM_SRV/{caseStudy}_FSet(Id=2,User='{username.ToUpper().Trim()}')?$format=json&sap-client={clientId}";
        }

        private async Task<JToken> getBodyContent(HttpResponseMessage response)
        {
            var stringContent = await response.Content.ReadAsStringAsync();
            var jsonContent = JObject.Parse(stringContent);
            return jsonContent["d"];
        }
    }
}