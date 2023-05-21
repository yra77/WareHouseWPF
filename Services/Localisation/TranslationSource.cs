

using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows.Data;


namespace WareHouseWPF.Services.Localisation
{
    internal class TranslationSource : INotifyPropertyChanged, ITranslationSource
    {
        private static TranslationSource instance = null;// new TranslationSource();

        public TranslationSource()
        {
            instance = this;
        }

        public static TranslationSource Instance
        {
            get { return instance; }
        }

        private readonly ResourceManager resManager = Properties.Resources.ResourceManager;
        private CultureInfo currentCulture = null;

        public string this[string key]
        {
            get { return this.resManager.GetString(key, this.currentCulture); }
        }

        public CultureInfo CurrentCulture
        {
            get { return this.currentCulture; }
            set
            {
                if (this.currentCulture != value)
                {
                    this.currentCulture = value;
                    var _event = this.PropertyChanged;
                    if (_event != null)
                    {
                        _event.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class LocExtension : Binding
    {
        public LocExtension(string name)
            : base("[" + name + "]")
        {
            this.Mode = BindingMode.OneWay;
            this.Source = TranslationSource.Instance;
        }
    }
}
