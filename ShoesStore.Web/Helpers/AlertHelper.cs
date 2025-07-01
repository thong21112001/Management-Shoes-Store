using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using ShoesStore.Application.Common.Models;

namespace ShoesStore.Web.Helpers
{
    public static class AlertHelper
    {
        private const string TempDataKey = "Alerts";

        public static void AddAlert(ITempDataDictionary tempData, AlertMessage alert)
        {
            var existing = new List<AlertMessage>();

            if (tempData.ContainsKey(TempDataKey))
            {
                var json = tempData[TempDataKey]?.ToString();
                if (!string.IsNullOrEmpty(json))
                {
                    existing = JsonConvert.DeserializeObject<List<AlertMessage>>(json) ?? new List<AlertMessage>();
                }
            }

            existing.Add(alert);
            tempData[TempDataKey] = JsonConvert.SerializeObject(existing);
        }

        public static List<AlertMessage> GetAlerts(ITempDataDictionary tempData)
        {
            if (tempData.ContainsKey(TempDataKey))
            {
                var json = tempData[TempDataKey]?.ToString();
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<List<AlertMessage>>(json) ?? new List<AlertMessage>();
                }
            }
            return new List<AlertMessage>();
        }
    }
}
