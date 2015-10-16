using System;

namespace Pinger
{
    class ValidationRule
    {
        public ValidationRule(string columnName, Func<string> rule)
        {
            _columnName = columnName;
            _rule = rule;
        }        
        
        private string _columnName;
        private Func<string> _rule;

        public string ColumnName
        {
            get
            {
                return _columnName;
            }
        }

        public Func<string> Rule
        {
            get
            {
                return _rule;
            }
        }
    }
}
