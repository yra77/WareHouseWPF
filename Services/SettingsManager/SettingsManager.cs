

using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;


namespace WareHouseWPF.Services.SettingsManager
{
    internal class SettingsManager : ISettingsManager
    {

        public CultureInfo Language { get; set; }
        public string Theme { get; set; }


        public bool SaveProperty(CultureInfo language, string theme = "Light")
        {

            Language = language;
            Theme = theme;

            try
            {
                File.WriteAllText(GetLocalFilePath("AppSettings.json"), JsonConvert.SerializeObject(this));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public SettingsManager Load()
        {
            try
            {
                var data = JsonConvert.DeserializeObject<SettingsManager>(File.ReadAllText(GetLocalFilePath("AppSettings.json")));

                return data;
            }
            catch
            {
                return new SettingsManager() { Language = new CultureInfo("uk"), Theme = "Light" };
            }
        }

        private string GetLocalFilePath(string fileName)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, fileName);
        }
    }
}
