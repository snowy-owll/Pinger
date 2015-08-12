using System;
using System.Windows.Input;

namespace Pinger
{
    class Command: ICommand
    {
        public Command(Action action, bool canExecute = true)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public Command(Action<object> action, bool canExecute = true)
        {
            _parameterizedAction = action;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        bool ICommand.CanExecute(object parameter)
        {
            return _canExecute;
        }

        public bool CanExecute
        {
            get { return _canExecute; }
            set
            {
                if (_canExecute != value)
                {
                    _canExecute = value;                    
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        void ICommand.Execute(object parameter)
        {
            if (_action != null)
                _action();
            else if (_parameterizedAction != null)
            {
                _parameterizedAction(parameter);
            }
        }

        private readonly Action _action = null;
        private readonly Action<object> _parameterizedAction = null;
        private bool _canExecute;
    }
}
