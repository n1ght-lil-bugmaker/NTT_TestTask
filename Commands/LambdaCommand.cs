using System;

namespace MVVM_SandBox.Commands
{
    /// <summary>
    /// Класс, использующий системные делегаты для выполнения различных команд
    /// </summary>
    internal class LambdaCommand : CommandBase
    {
        /// <summary>
        /// Делагат, определяющий возможность выполнения команды
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        /// <summary>
        /// Делегат, выполняющий команду
        /// </summary>
        private readonly Action<object> _execute;

        /// <summary>
        /// Устанавливает делагаты, необходимые для работы команды
        /// </summary>
        /// <param name="canExecute">Делегат, проверяющий возможность выполнения команды</param>
        /// <param name="execute">Делегат, выполняющий команду</param>
        public LambdaCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        /// <summary>
        /// Проверяет возможность выполняния команды, используя вышеописанные делегаты
        /// </summary>
        /// <param name="parameter">Параметр проверки</param>
        /// <returns></returns>
        public override bool CanExecute(object parameter) => _canExecute(parameter);

        /// <summary>
        /// Выполняет команду, используя вышеописанный делегат
        /// </summary>
        /// <param name="parameter">Параметр выполнения</param>
        public override void Execute(object parameter)
        {
            if (_canExecute(parameter))
            {
                _execute(parameter);
            }
        }
    }
}
