using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace Cotizador.Common.Validations
{
    public class CamposVaciosValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return String.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Campo requerido.")
                : ValidationResult.ValidResult;
        }
    }
}
