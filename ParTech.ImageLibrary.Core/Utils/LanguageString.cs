using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace ParTech.ImageLibrary.Core.Utils
{
    public static class LanguageString
    {
        public static string GetStringForCurrentLanguage(string jsonString)
        {
            return GetStringForCurrentLanguage(Thread.CurrentThread.CurrentUICulture.ThreeLetterISOLanguageName,
                jsonString);
        }

        public static string GetStringForCurrentLanguage(string language, string jsonString)
        {
            var output = string.Empty;

            try
            {
                var serializer = new DataContractJsonSerializer(typeof(Classes.LanguageString));
                var stream = GenerateStreamFromString(jsonString);
                var jsonResult = (Classes.LanguageString)serializer.ReadObject(stream);

                var languageItem = jsonResult.Values.FirstOrDefault(l => l.Code == language);
                if (languageItem != null)
                {
                    output = languageItem.Value;
                }
            }
            catch (Exception ex)
            {
                
            }

            return output;
        }

        private static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}
