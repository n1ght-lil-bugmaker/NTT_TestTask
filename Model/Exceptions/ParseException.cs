using System;

namespace MVVM_SandBox.Model.Exceptions
{
    /// <summary>
    /// Описывает ошибку парсинга данных
    /// </summary>
    public class ParseException : Exception
    {
        private string _wrongString;
        public override string Message => "Ошибка обработки данных. Проверьте целевой файл\nСтрока: " + _wrongString;

        public ParseException(string wrongString)
        {
            _wrongString = wrongString;
        }
    }
}
