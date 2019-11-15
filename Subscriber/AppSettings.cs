using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    public class AppSettings
    {
        public virtual TimeSpan BatchTimeout => TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["BatchTimeoutInMinutes"] ?? "1"));

        public virtual DateTime BatchCutOffTime => DateTime.ParseExact(ConfigurationManager.AppSettings["BatchCutOffTime"] ?? "120000", "HHmmss", CultureInfo.InvariantCulture);

        public virtual int MaximumBatchSize => int.Parse(ConfigurationManager.AppSettings["MaximumBatchSize"] ?? "2");

        public virtual DateTime BatchCutOffDateTime => new DateTime(DateTime.Today.Year,
            DateTime.Today.Month,
            DateTime.Today.Day,
            BatchCutOffTime.Hour,
            BatchCutOffTime.Minute,
            BatchCutOffTime.Second);

        public virtual string NServiceBusPersistence => ConfigurationManager.AppSettings["NServiceBusPersistence"];

        public virtual int MaxImmediateRetries => int.Parse(GetStringOrDefault("MaxImmediateRetries", null));

        public virtual int MaxDelayedRetries => int.Parse(GetStringOrDefault("MaxDelayedRetries", null));

        private static string GetStringOrDefault(string key,
            string defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];

            return string.IsNullOrEmpty(value) ?
                defaultValue :
                value;
        }
    }
}
