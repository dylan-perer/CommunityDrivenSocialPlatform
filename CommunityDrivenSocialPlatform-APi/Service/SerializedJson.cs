using Nancy.Json;
using System.Collections.Generic;

namespace CommunityDrivenSocialPlatform_APi.Service
{
    public static class SerializedJson
    {
        public static object SerializedJsonList(IDictionary<string, object> dictionary)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonStr = "{";

            foreach (KeyValuePair<string, object> kvp in dictionary)
            {
                jsonStr += $"\"{kvp.Key}\":\"{kvp.Value}\",";
            }

            jsonStr = jsonStr.Substring(0, jsonStr.Length - 1);
            jsonStr += "}";

            return serializer.Deserialize<object>(jsonStr);
        }


    }
}
