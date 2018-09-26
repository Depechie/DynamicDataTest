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
        private CachingService _cachingService;
        private StravaService _stravaService;

        private ActivityIncrementalLoading _activities = new ActivityIncrementalLoading();
        public ActivityIncrementalLoading Activities => _activities;

        private readonly IComparer<ActivitySummary> _comparer = SortExpressionComparer<ActivitySummary>.Descending(item => item.StartDate);

        public MainViewModel(StravaService stravaService)
        {
            _stravaService = stravaService;
            _cachingService = _activities.CachingService = new CachingService(_stravaService);

            var loader = _activities.CachedItems.Connect()
                    .Sort(_comparer)
                    .ObserveOnDispatcher()
                    .Bind(_activities)
                    .Subscribe();
        }
    }
}
