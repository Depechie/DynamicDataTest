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
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ActivityMeta)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}