

using System.Globalization;


namespace WareHouseWPF.Services.Localisation
{
    public interface ITranslationSource
    {
        CultureInfo CurrentCulture { get; set; }
    }
}
