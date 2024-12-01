using System;
using System.Windows.Input;

namespace todolistmanagercsharp.ViewModels
{


    // Task RelayCommand ICommand
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            // Debugging output (optional)
            Console.WriteLine($"RelayCommand.CanExecute invoked. CanExecute result: {_canExecute?.Invoke() ?? true}");
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            // Debugging output (optional)
            Console.WriteLine("Executing task command...");
            _execute();
        }
    }



    // Task Relay <T> ICommand
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            // Validate parameter type and check condition
            bool canExecuteResult = _canExecute == null || (parameter is T param && _canExecute(param));
            // Debugging output (optional)
            Console.WriteLine($"RelayCommand<T>.CanExecute invoked. Parameter: {parameter}, CanExecute result: {canExecuteResult}");
            return canExecuteResult;
        }

        public void Execute(object parameter)
        {
            if (parameter is T param)
            {
                // Debugging output (optional)
                Console.WriteLine($"RelayCommand<T>.Execute invoked with parameter: {param}");
                _execute(param);
            }
            else if (parameter == null && typeof(T).IsClass) // Handle null parameters for reference types
            {
                Console.WriteLine("RelayCommand<T>.Execute invoked with null parameter.");
                _execute(default);
            }
            else
            {
                throw new InvalidOperationException($"Invalid parameter type. Expected: {typeof(T)}, Received: {parameter?.GetType()}");
            }
        }
    }
}
