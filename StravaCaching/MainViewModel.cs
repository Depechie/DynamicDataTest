using DynamicData;
using StravaNetStandard.Models;
using StravaNetStandard.Services;
using System.Collections.Generic;
using System;
using System.Reactive.Linq;
using StravaCaching.Services;
using DynamicData.Binding;
using StravaCaching.Models;

namespace StravaCaching
{
    public class MainViewModel : BaseClass
    {
        private ActivityIncrementalLoading _activities;
        public ActivityIncrementalLoading Activities => _activities;

        private readonly IComparer<ActivitySummary> _comparer = SortExpressionComparer<ActivitySummary>.Descending(item => item.StartDate);

        public MainViewModel(StravaService stravaService)
        {
            _activities = new ActivityIncrementalLoading(new CachingService(stravaService));

            var loader = _activities.CachedItems.Connect()
                    .Sort(_comparer)
                    .ObserveOnDispatcher()
                    .Bind(_activities)
                    .Subscribe();
        }
    }
}
