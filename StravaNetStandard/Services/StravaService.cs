using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using StravaNetStandard.Models;

namespace StravaNetStandard.Services
{
    public class StravaService
    {
        private string _stravaAuthorityAuthorizeURL = "https://www.strava.com/oauth/authorize";
        private string _stravaAuthorityTokenURL = "https://www.strava.com/oauth/token";
        private string _stravaAuthorityClientID = "28496";
        private string _stravaAuthorityClientSecret = "2d24e9a1f492323f73edc3f1729004d0af16750f";

        private StravaWebClient _stravaWebClient;

        public string AccessToken { get; set; }

        public string AuthorityRedirectURL => "http://www.versweyveld.info";

        public string AuthorizationURL => $"{_stravaAuthorityAuthorizeURL}?client_id={_stravaAuthorityClientID}&response_type=code&redirect_uri={AuthorityRedirectURL}&scope=view_private,write&state=mystate&approval_prompt=force";

        public StravaService()
        {
            _stravaWebClient = new StravaWebClient();
        }

        public string ParseAuthorizationResponse(string responseData)
        {
            var authorizationCodeIndex = responseData.ToLower().IndexOf("&code=") + 6;
            return responseData.Substring(authorizationCodeIndex, responseData.Length - authorizationCodeIndex);
        }

        public async Task<string> GetAccessToken(string authorizationCode)
        {
            try
            {
                var values = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("client_id", _stravaAuthorityClientID),
                        new KeyValuePair<string, string>("client_secret", _stravaAuthorityClientSecret),
                        new KeyValuePair<string, string>("code", authorizationCode)
                    };

                var httpClient = new HttpClient(new HttpClientHandler());
                var response = await httpClient.PostAsync(_stravaAuthorityTokenURL, new FormUrlEncodedContent(values));
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();

                var accessToken = JsonConvert.DeserializeObject<AccessToken>(responseString);
                return AccessToken = accessToken.Token;
            }
            catch (Exception ex)
            {
                //TODO: Report error!
            }

            return AccessToken;
        }

        public async Task<string> GetFriendActivityDataAsync(int page, int perPage)
        {
            string data = null;

            try
            {
                //TODO: Glenn - Optional parameters should be treated as such!
                string getUrl = $"{Endpoints.Activities}?page={page}&per_page={perPage}"; // &access_token={AccessToken}";
                data = await _stravaWebClient.GetAsync(new Uri(getUrl), AccessToken);
            }
            catch (Exception ex)
            {
                //TODO: Report error!
            }

            return data;
        }
    }
}
