

using System.Globalization;


namespace WareHouseWPF.Services.SettingsManager
{
    internal interface ISettingsManager
    {
        bool SaveProperty(CultureInfo language, string theme = "Light");
        SettingsManager Load();
    }
}
