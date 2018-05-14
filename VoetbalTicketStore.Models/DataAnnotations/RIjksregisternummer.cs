using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VoetbalTicketStore.Models.DataAnnotations
{
    public class Rijksregisternummer : ValidationAttribute, IClientValidatable
    {
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule mvr = new ModelClientValidationRule();
            mvr.ErrorMessage = "Birth Date can not be greater than current date";
            mvr.ValidationType = "validbirthdate";
            return new[] { mvr };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //return base.IsValid(value, validationContext);

            string input = value.ToString();


            if (input.Length == 11 && Int32.TryParse(input.Substring(0, 9), out int deel1) && Int32.TryParse(input.Substring(9, 2), out int controlegetal))
            {

                if ((97 - (deel1 % 97) == controlegetal) || (97 - ((2000000000 + deel1) % 97) == controlegetal))
                {
                    // geldig rrn
                    return ValidationResult.Success;
                }
                else
                {
                    // cijfers kloppen niet
                    return new ValidationResult("Ongeldig gevormd rijksregisternummer!");
                }

            }
            else
            {
                // ongeldig rrn - parsing loopt mis
                return new ValidationResult("Ongeldig gevormd rijksregisternummer!");
            }
        }
    }
}

