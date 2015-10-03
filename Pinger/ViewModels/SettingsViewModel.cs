using Pinger.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Pinger.ViewModels
{
    class SettingsViewModel:ObservableObject
    {
        public SettingsViewModel(Settings settings)
        {
            _settings = settings;
            _soundPing = _settings.SoundPing;
            _supportedCultures = new ObservableCollection<CultureItem>();
            SupportedCultures.Add(new CultureItem(CultureInfo.InstalledUICulture, true));
            ResourceManager rm = new ResourceManager(typeof(Localization.Localization));
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo culture in cultures)
            {
                try
                {
                    ResourceSet rs = rm.GetResourceSet(culture, true, false);
                    if (rs == null || culture.Equals(CultureInfo.InvariantCulture)) continue;
                    SupportedCultures.Add(new CultureItem(culture));
                }
                catch (CultureNotFoundException)
                {
                    Debug.WriteLine(culture + " is not available on the machine or is an invalid culture identifier.");
                }
            }
            CurrentCulture = SupportedCultures.Where(c => c.CultureInfo.Equals(_settings.Language.CultureInfo)).First();
        }

        private Settings _settings = new Settings();
        
        #region Properties
        private SoundPing _soundPing;
        private CultureItem _currentCulture;
        private ObservableCollection<CultureItem> _supportedCultures;

        public SoundPing SoundPing
        {
            get { return _soundPing; }
            set
            {
                if (value != _soundPing)
                {
                    _soundPing = value;
                    _settings.SoundPing = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<CultureItem> SupportedCultures
        {
            get { return _supportedCultures; }
            set
            {
                if (value != _supportedCultures)
                {
                    _supportedCultures = value;
                    OnPropertyChanged();
                }
            }
        }

        public CultureItem CurrentCulture
        {
            get { return _currentCulture; }
            set
            {
                if (value != _currentCulture)
                {
                    _currentCulture = value;
                    _settings.Language = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion
    }
}
