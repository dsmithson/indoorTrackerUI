using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IndoorLocationViewer
{
    public static class DeviceHelper
    {
        static readonly ConnectionMultiplexer redis;
        static readonly IDatabaseAsync db;
        static DeviceHelper()
        {
            redis = ConnectionMultiplexer.Connect("ds-ubuntu");
            db = redis.GetDatabase();
        }

        public static async Task<string> LookupVendorByMac(string mac)
        {
            //Ensure we have a valid mac
            if (string.IsNullOrEmpty(mac) || mac.Split(":").Length != 6)
                return string.Empty;

            //Get mac organization bytes
            string org = mac.ToUpper().Replace(":", "").Substring(0, 6);
            string cacheKey = "IndoorLocation.Vendors." + org;

            //Check if value is in cache
            string cached = await db.StringGetAsync(org).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(cached))
                return cached;

            string parseStart1 = $"<span>{org}</span>";
            const string parseStart2 = ">";
            const string parseEnd = "</a>";

            string uri = "https://www.adminsub.net/mac-address-finder/" + mac.ToUpper();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(uri).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                int startIndex = responseData.IndexOf(parseStart1);
                if (startIndex >= 0)
                {
                    startIndex = responseData.IndexOf(parseStart2, startIndex + parseStart1.Length);
                    if (startIndex >= 0)
                    {
                        startIndex += parseStart2.Length;
                        int endIndex = responseData.IndexOf(parseEnd, startIndex);
                        if (endIndex >= 0)
                        {
                            string manufacturer = responseData.Substring(startIndex, (endIndex - startIndex));

                            //Store result in cache
                            await db.StringSetAsync(cacheKey, manufacturer);
                            return manufacturer;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
