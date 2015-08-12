using Infralution.Localization.Wpf;
using System.Globalization;

namespace Pinger.Models
{
    class CultureItem : ObservableObject
    {
        public CultureItem(CultureInfo info)
            : this(info, false)
        {

        }

        public CultureItem(CultureInfo info, bool system)
        {
            _cultureInfo = info;
            if (system)
            {
                CultureManager.UICultureChanged += (s, e) =>
                    {
                        DisplayCultureName = Localization.Localization.SystemLanguage;
                    };
                DisplayCultureName = Localization.Localization.SystemLanguage;
            }
            else
            {
                _displayCultureName = info.NativeName;
            }
        }

        private CultureInfo _cultureInfo;

        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            private set { _cultureInfo = value; OnPropertyChanged(); }
        }

        private string _displayCultureName;

        public string DisplayCultureName
        {
            get { return _displayCultureName; }
            private set { _displayCultureName = value; OnPropertyChanged(); }
        }

        private bool _system;

        public bool System
        {
            get { return _system; }
            set { _system = value; OnPropertyChanged(); }
        }


    }
}
