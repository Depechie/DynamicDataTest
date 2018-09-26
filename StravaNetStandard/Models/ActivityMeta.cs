using Newtonsoft.Json;
using System;

namespace StravaNetStandard.Models
{
    public class ActivityMeta : BaseClass, IEquatable<ActivityMeta>
    {
        /// <summary>
        /// The id of the activity. This id is provided by Strava at upload.
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        public bool Equals(ActivityMeta other)
        {
            if (other == null)
                return false;

            //TODO: Glenn - For now, as test, we will assume that the Id itself is our Equal check ( add extra properties in future )
            return Id.Equals(other.Id);            
        }
    }
}
