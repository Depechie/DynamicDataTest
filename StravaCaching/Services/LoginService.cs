using Microsoft.Toolkit.Uwp;
using StravaCaching.Models;
using StravaNetStandard.Services;
using System;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;

namespace StravaCaching.Services
{
    public class LoginService
    {
        private StravaService _stravaService;

        public LoginService(StravaService stravaService)
        {
            _stravaService = stravaService;
        }

        public async Task Login()
        {
            try
            {
                WebAuthenticationResult webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(_stravaService.AuthorizationURL), new Uri(_stravaService.AuthorityRedirectURL));

                if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    var responseData = webAuthenticationResult.ResponseData;
                    var tempAuthorizationCode = _stravaService.ParseAuthorizationResponse(responseData);
                    var accessToken = await _stravaService.GetAccessToken(tempAuthorizationCode);

                    //TODO: Store accessToken in secure location, needed for each Strava Service request!
                    var storageHelper = new LocalObjectStorageHelper();
                    storageHelper.Save<string>(StorageKeys.ACCESSTOKEN, accessToken);
                }
            }
            catch (Exception ex)
            {
                //OnStatusEvent(new StravaServiceEventArgs(StravaServiceStatus.Failed, ex));
            }
        }
    }
}
