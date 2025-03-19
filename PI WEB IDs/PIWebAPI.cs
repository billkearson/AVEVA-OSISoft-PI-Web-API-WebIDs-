using System;
using System.Net;
using System.Net.Security;
using System.Text;


namespace PI_WEB_IDs
{
    class PIWebAPI
    {

        #region Local
        private string username = "piuser";
        private string password = "password";
        private string piapiserver = "mypiapiserver";
        private string piapiserverport = "443";
        private string piwebapiurlbase = $"https://mypiapiserver:443/piwebapi";
        private string piserver = "mypiserver";
        private bool debug = true;
        private bool enablesslvalidation = true;
        private bool usedefaultcredentials = true;

        #endregion

        #region Properties

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        public string PIAPIServer
        {
            get
            {
                return piapiserver;
            }
            set
            {
                piapiserver = value;
                piwebapiurlbase = $"https://{piapiserver}:{piapiserverport}/piwebapi";
            }

        }

        public string PIAPIServerPort
        {
            get
            {
                return piapiserverport;
            }
            set
            {
                piapiserverport = value;
                piwebapiurlbase = $"https://{piapiserver}:{piapiserverport}/piwebapi";
            }

        }

        public string PIServer
        {
            get
            {
                return piserver;
            }
            set
            {
                piserver = value;                
            }

        }

        public bool Debug
        {
            get
            {
                return debug;
            }
            set
            {
                debug = value;
            }

        }

        public bool EnableSslValidation
        {
            get
            {
                return enablesslvalidation ;
            }
            set
            {
                enablesslvalidation = value;
            }

        }

        public bool UseDefaultCredentials
        {
            get
            {
                return usedefaultcredentials;
            }
            set
            {
                usedefaultcredentials = value;
            }

        }

        #endregion

        #region Methods
        // Method to write data to the PI Server when the WebID is known
        public async Task<string> WriteValueToWebID(string PIPointWebId, string Value, string Timestamp)
        {
            // PI Web API URL for writing data to a PI Point
            string DataUrl = $"{piwebapiurlbase}/streams/{PIPointWebId}/value";

            // Create the JSON payload to send (value and timestamp)                
            string Data = $"{{\"Value\":{Value}, \"Timestamp\":\"{Timestamp}\"}}";

            string response = await PIAsyncWebCallPost(DataUrl, Data);
            return response;

        }

        // Method to write data to the PI Server when the WebID is known
        public async Task<string> WriteValueToPIPoint(string PIPoint, string Value, string Timestamp)
        {
            // PI Web API URL for writing data to a PI Point
            string PIPointWebID = CreatePIPointPathWebID(piapiserver, PIPoint);
            string DataUrl = $"{piwebapiurlbase}/streams/{PIPointWebID}/value";

            // Create the JSON payload to send (value and timestamp)                
            string Data = $"{{\"Value\":{Value}, \"Timestamp\":\"{Timestamp}\"}}";

            string response = await PIAsyncWebCallPost(DataUrl, Data);
            return response;

        }

        // Method to get PIPoint information
        public async Task<string> ReadAttributesFromPIPoint(string PIPoint)
        {
            // PI Web API URL for writing data to a PI Point
            string PIPointWebID = CreatePIPointPathWebID(piapiserver, PIPoint);
            string DataUrl = $"{piwebapiurlbase}/points/{PIPointWebID}";
                      
            string response = await PIAsyncWebCallGet(DataUrl);
            return response;

        }

        // Method to get PIPoint information
        public async Task<string> ReadAttributedsFromWebID(string PIPointWebId)
        {
            // PI Web API URL for writing data to a PI Point           
            string DataUrl = $"{piwebapiurlbase}/points/{PIPointWebId}";

            string response = await PIAsyncWebCallGet(DataUrl);
            return response;

        }

        // Method to get PIPoint current value
        public async Task<string> ReadCurrentValueFromPIPoint(string PIPoint)
        {
            // PI Web API URL for writing data to a PI Point
            string PIPointWebID = CreatePIPointPathWebID(piapiserver, PIPoint);
            string DataUrl = $"{piwebapiurlbase}/streams/{PIPointWebID}/Value";

            string response = await PIAsyncWebCallGet(DataUrl);
            return response;

        }

        // Method to get PIPoint current value
        public async Task<string> ReadCurrentValueFromWebID(string PIPointWebId)
        {
            // PI Web API URL for writing data to a PI Point           
            string DataUrl = $"{piwebapiurlbase}/streams/{PIPointWebId}/Value";

            string response = await PIAsyncWebCallGet(DataUrl);
            return response;

        }

        // Create a PathOnly WebID for a PI Server adn PITag
        public string CreatePIPointPathWebID(string PIServer, string PIPoint)
        {
            string type = "P"; // Spsecify Path-only
            string version = "1"; // eb 2.0 = 1
            string marker = "DP"; // Specify the object type as PIPoint
            string path = @$"{PIServer}\{PIPoint}"; // PIServer and PI Tag name

            // Get the encoded basr64 value for the path 
            string encoded = Encode(path);

            // Build the WebId and the URL
            string webid = $"{type}{version}{marker}{encoded}";

            return webid;

        }

        // Encode the path string (server\tagname) to base64 and replace special characters
        private static string Encode(string PathOnly)
        {
            byte[] value = System.Text.Encoding.UTF8.GetBytes(PathOnly.ToUpperInvariant());
            string encoded = System.Convert.ToBase64String(value);

            // Remove Special Characters
            encoded = encoded.TrimEnd(new char[] { '=' }).Replace('+', '-').Replace('/', '_');

            return encoded;

        }

        // Async web call provided the URL and Data
        private async Task<string> PIAsyncWebCallPost(string URL, string Data)
        {
            try
            {
                using (HttpClientHandler handler = CreateHttpClientHandler())
                using (HttpClient client = new HttpClient(handler))
                {

                    if (usedefaultcredentials)
                    {
                        // Set authentication to kerberos
                        handler.UseDefaultCredentials = true;
                    }
                    else
                    {
                        // Set the authentication (basic auth)
                        var byteArray = new System.Text.ASCIIEncoding().GetBytes($"{username}:{password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }

                    // Add security headers
                    client.DefaultRequestHeaders.Add("X-Requested-With", "Accept");
                    //client.DefaultRequestHeaders.Add("Accept", "application/json");

                    StringContent content = new StringContent(Data, Encoding.UTF8, "application/json");

                    // Make the request 
                    HttpResponseMessage response = await client.PostAsync(URL, content);

                    if (response.IsSuccessStatusCode)
                    {
                        if (debug) Console.WriteLine("Data written successfully!");
                        return "Ok";
                    }
                    else
                    {
                        Console.WriteLine($"Failed to write data. Status Code: {response.StatusCode}");
                        string responseContent = await response.Content.ReadAsStringAsync();
                        if (debug) Console.WriteLine($"Response Content: {responseContent}");
                        return responseContent;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }           

        }

        private async Task<string> PIAsyncWebCallGet(string URL)
        {
            try
            {
                string responsebody = string.Empty;

                using (HttpClientHandler handler = CreateHttpClientHandler())
                using (HttpClient client = new HttpClient(handler))
                {       
                    if (usedefaultcredentials)
                    {
                        // Set authentication to kerberos
                        handler.UseDefaultCredentials = true;
                    }
                    else
                    {
                        // Set the authentication (basic auth)
                        var byteArray = new System.Text.ASCIIEncoding().GetBytes($"{username}:{password}");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    }                            

                    // Add security headers
                    client.DefaultRequestHeaders.Add("X-Requested-With", "Accept");
                    //client.DefaultRequestHeaders.Add("Accept", "application/json");

                    // Make the request 
                    HttpResponseMessage response = await client.GetAsync(URL);

                    if (response.IsSuccessStatusCode)
                    {
                        if (debug) Console.WriteLine("Data written successfully!");
                        responsebody = await response.Content.ReadAsStringAsync();
                        return responsebody;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to write data. Status Code: {response.StatusCode}");
                        string responseContent = await response.Content.ReadAsStringAsync();
                        if (debug) Console.WriteLine($"Response Content: {responseContent}");
                        return responseContent;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            

        }

        // This method configures SSL certificate validation based on the enableSslValidation flag
        private HttpClientHandler CreateHttpClientHandler()
        {
            var handler = new HttpClientHandler();

            // Configure SSL certificate validation based on the enableSslValidation flag
            if (enablesslvalidation)
            {
                // Use the default validation callback (trusted certificates only)
                handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
                {
                    // Only allow trusted certificates (no errors)
                    return sslPolicyErrors == SslPolicyErrors.None;
                };
            }
            else
            {
                // Disable SSL certificate validation, accepting all certificates including self-signed
                handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true; // Always return true to bypass errors
            }
                    
            return handler;
        }


        #endregion

    }
}
