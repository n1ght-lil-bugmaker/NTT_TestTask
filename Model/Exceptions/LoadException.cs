using System;

namespace MVVM_SandBox.Model.Exceptions
{
    /// <summary>
    /// Описывает ошибку загрузки файла
    /// </summary>
    public class LoadException : Exception
    {
        public override string Message => "Ошибка загрузки";
    }
}
