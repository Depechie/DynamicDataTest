using DynamicData;
using DynamicData.Binding;
using StravaNetStandard.Models;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace StravaCaching.Models
{
    public abstract class CachedIncrementalLoadingBase<T> : ObservableCollectionExtended<T>, ISupportIncrementalLoading where T:ActivityMeta
    {
        private int _page = 1;
        private bool _loading;

        protected abstract Task<int> FetchData(int page, int pageSize);

        public SourceCache<T, long> CachedItems = new SourceCache<T, long>(item => item.Id);

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            _loading = true;
            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        public bool HasMoreItems => true;

        private async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken c, uint count)
        {
            try
            {
                int dataCount = await FetchData(_page, 30);

                if (dataCount > 0)
                    ++_page; //TODO: Tell cache to NO LONGER trigger next fetch > Hasmoreitems!

                return new LoadMoreItemsResult() { Count = (uint)dataCount };
            }
            finally
            {
                _loading = false;
            }
        }

        //TODO: Glenn - implement has HasMoreItems property??
    }
}
