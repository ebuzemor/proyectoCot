using System;
using System.Globalization;
using System.Windows.Controls;

namespace Cotizador.Common.Validations
{
    public class FechaAntesValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime time;
            if (!DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out time)) return new ValidationResult(false, "Ingrese una fecha con el formato dd/mm/yyyy");

            return time.Date < DateTime.Now.Date
                ? new ValidationResult(false, "No puede elegir una fecha anterior a la de hoy")
                : ValidationResult.ValidResult;
        }
    }
}
