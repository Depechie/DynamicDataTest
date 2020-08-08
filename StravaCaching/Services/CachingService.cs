using Microsoft.Toolkit.Uwp;
using Newtonsoft.Json;
using StravaCaching.Models;
using StravaNetStandard.Models;
using StravaNetStandard.Services;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace StravaCaching.Services
{
    public class CachingService
    {
        private StravaService _stravaService;
        private LocalObjectStorageHelper _storageHelper = new LocalObjectStorageHelper();

        private DateTimeOffset? _offSet;

        public CachingService(StravaService stravaService)
        {
            _stravaService = stravaService;
        }

        public IObservable<string> GetActivities(int page, int pageSize)
        {
            //if (page == 1)
            //    return Observable.Concat(GetFriendsAvtivitiesDataCache().ToObservable(), GetFriendsActivitiesWeb(page, pageSize).ToObservable());
            //else
                return GetFriendsActivitiesWeb(page, pageSize).ToObservable();
        }

        public Task<string> GetFriendsAvtivitiesDataCache()
        {
            return Task.Run(async () =>
            {
                string activities = string.Empty;

                if (await _storageHelper.FileExistsAsync(StorageKeys.ACTIVITIESALL))
                    activities = await _storageHelper.ReadFileAsync<string>(StorageKeys.ACTIVITIESALL);

                return activities;
            });
        }

        public Task<string> GetFriendsActivitiesWeb(int page, int pageSize)
        {
            return Task.Run(async () =>
            {
                //await Task.Delay(3000);

                string activities = await _stravaService.GetFriendActivityDataAsync(page, pageSize);
                if(page == 1) //TODO: Glenn - we only cache the first page ( these are always the newest items )
                    await _storageHelper.SaveFileAsync(StorageKeys.ACTIVITIESALL, activities);

                return activities;
            });
        }
    }
}