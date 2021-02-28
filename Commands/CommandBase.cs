using System;
using System.Windows.Input;

namespace MVVM_SandBox.Commands
{
    /// <summary>
    /// Базовый класс для команд
    /// </summary>
    internal abstract class CommandBase : ICommand
    {
        /// <summary>
        /// Определяет событие изменения возможности выполнения команды, используя CommandManager
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Определяет возможность выполнения данной команды
        /// </summary>
        /// <param name="parameter">Параметр проверки</param>
        /// <returns>Может ли команда быть выполнена</returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// Выполняет команду
        /// </summary>
        /// <param name="parameter">Параметр выполнения</param>
        public abstract void Execute(object parameter);
    }
}
