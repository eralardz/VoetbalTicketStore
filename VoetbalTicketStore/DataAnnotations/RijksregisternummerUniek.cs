using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VoetbalTicketStore.Service;

namespace VoetbalTicketStore.DataAnnotations
{
    public class RijksregisternummerUniek : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string input = value.ToString();

            UserService userService = new UserService();
            if (userService.RijksregisternummerKomtAlVoor(input))
            {
                return new ValidationResult("Dit rijksregisternummer komt al voor in ons systeem!");
            }

            else
            {
                return ValidationResult.Success;
            }
        }
    }
}