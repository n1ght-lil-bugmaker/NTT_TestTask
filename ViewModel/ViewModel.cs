using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM_SandBox.ViewModel
{
    /// <summary>
    /// Базовый класс для модели отображения
    /// </summary>
    internal abstract class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения чего-либо
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает срабатывания события PropertyChanged
        /// </summary>
        /// <param name="propertyName">Название изменившегося свойства</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Изменяет свойство с вызовом события PropertyChanged
        /// </summary>
        /// <typeparam name="T">Любой тип данных</typeparam>
        /// <param name="field">Ссылка на изменияемое поле</param>
        /// <param name="value">Новое значение</param>
        /// <param name="propertyName">Название изменившегося свойства</param>
        /// <returns></returns>
        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if(Equals(field, value))
            {
                return false;
            }
            else
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
    }
}
