using Nancy.Json;
using System;
using System.Collections.Generic;

namespace TestingShit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("txt", "notepad.exe");
            dictionary.Add("bmp", 23);
            dictionary.Add("dib", "paint.exe");
            dictionary.Add("rtf", "wordpad.exe");
            SerializedJsonList(dictionary);
        }

        public static object SerializedJsonList(IDictionary<string, object> dictionary)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonStr="{";

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
