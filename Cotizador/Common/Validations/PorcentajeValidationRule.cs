using System.Globalization;
using System.Windows.Controls;

namespace Cotizador.Common.Validations
{
    public class PorcentajeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string valor = value.ToString();
            double descto;
            bool check = double.TryParse(valor, out descto);
            if (check == true && descto >= 0 && ((descto / 100.0) <= 0.9999))
                return ValidationResult.ValidResult;
            else
                return new ValidationResult(false, "Porcentaje no permitido");

        }
    }
}
