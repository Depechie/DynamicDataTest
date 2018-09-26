using DynamicData;
using StravaCaching.Services;
using StravaNetStandard.Helpers;
using StravaNetStandard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StravaCaching.Models
{
    public class ActivityIncrementalLoading : CachedIncrementalLoadingBase<ActivitySummary>
    {
        public CachingService CachingService { get; set; }

        protected override Task<int> FetchData(int page, int pageSize)
        {
            TaskCompletionSource<int> taskCompletionSource = new TaskCompletionSource<int>();

            CachingService.GetActivities(page, pageSize).Subscribe(result =>
            {
                if (!string.IsNullOrEmpty(result))
                {
                    IEnumerable<ActivitySummary> activities = Unmarshaller<IEnumerable<ActivitySummary>>.Unmarshal(result);
                    if (activities != null && activities.Any())
                        //CachedItems.EditDiff(activities, EqualityComparer<ActivitySummary>.Default);
                        CachedItems.AddOrUpdate(activities); //TODO: Glenn - The AddOrUpdate does look a bit to harsh - flashing list!

                    taskCompletionSource.TrySetResult(activities.Count());
                }

                taskCompletionSource.TrySetResult(0);
            });

            return taskCompletionSource.Task;
        }
    }
}
