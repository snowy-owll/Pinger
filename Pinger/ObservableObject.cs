using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Pinger
{
    class ObservableObject : INotifyPropertyChanged, IDataErrorInfo
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {            
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErrorInfo

        private List<ValidationRule> _validationRules = new List<ValidationRule>();

        protected void AddValidationRule(ValidationRule rule)
        {
            _validationRules.Add(rule);
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {                
                ValidationRule rule = _validationRules.Where(x => x.ColumnName == columnName).First();
                if(rule !=null)
                {                    
                    return rule.Rule();
                }
                return null;
            }
        }                

        string IDataErrorInfo.Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
