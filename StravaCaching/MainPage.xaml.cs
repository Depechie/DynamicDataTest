using Microsoft.Toolkit.Uwp;
using StravaCaching.Models;
using StravaCaching.Services;
using StravaNetStandard.Services;
using System.Collections.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StravaCaching
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LoginService _loginService;
        private StravaService _stravaService;

        public MainPage()
        {
            this.InitializeComponent();
            _stravaService = new StravaService();
            _loginService = new LoginService(_stravaService);
            DataContext = new MainViewModel(_stravaService);

            var storageHelper = new LocalObjectStorageHelper();
            if (storageHelper.KeyExists(StorageKeys.ACCESSTOKEN))
                _stravaService.AccessToken = storageHelper.Read<string>(StorageKeys.ACCESSTOKEN);
        }
    }
}
