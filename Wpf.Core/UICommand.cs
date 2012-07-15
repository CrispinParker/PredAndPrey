namespace PredAndPrey.Wpf.Core
{
    using System;
    using System.Windows.Input;

    internal class UICommand : ICommand
    {
        private readonly Action<object> executeMethod;

        private readonly Func<object, bool> canExecuteMethod;

        public UICommand(Action<object> executeMethod)
            : this(executeMethod, o => true)
        {
        }

        public UICommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.executeMethod(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecuteMethod(parameter);
        }
    }
}