using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using SignicatIdentification.Models;

namespace ConsoleApp
{
    class Program
    {
        
        const string identificationSession = "https://api.idfy.io";

        private static async Task<AccessTokenModel> GetToken()
        {
            //here I used my credentials instead of empty strings
            string clientId = "";
            string clientSecret = "";
            string credentials = String.Format("{0}:{1}", clientId, clientSecret);
           

            using (var client = new HttpClient())
            {
                //Define Headers
                client.BaseAddress = new Uri(identificationSession);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

                //Prepare Request Body
                List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
                requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                requestData.Add(new KeyValuePair<string, string>("scope", "identify"));

                FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

                //Request Token
                var request = await client.PostAsync("/oauth/connect/token", requestBody);
                var response = await request.Content.ReadAsStringAsync();
                AccessTokenModel token =  JsonConvert.DeserializeObject<AccessTokenModel>(response);

                return token;
            }
        }


        private static async Task<SessionModel> CreateSession()
        {

            var token = await GetToken();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(identificationSession);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);

                var jsonText = File.ReadAllText(Environment.CurrentDirectory + "/Session.json");

                var content = new StringContent(jsonText, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/identification/v2/sessions", content);
                response.EnsureSuccessStatusCode();

                string responseJson = await response.Content.ReadAsStringAsync();
                SessionModel session = JsonConvert.DeserializeObject<SessionModel>(responseJson);

                return session;
            }

        }


       public static async Task<string> Open()
        {
            
            SessionModel session = await CreateSession();

           
            if (session != null)
                return session.url;
            else return "false";

        }


        [STAThread]
        static void Main(string[] args)
        {
           

            String session = Open().Result;
            var ps = new ProcessStartInfo(session)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);

        }
        

        }
}

