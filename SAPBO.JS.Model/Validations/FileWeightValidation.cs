using Microsoft.AspNetCore.Http;
using SAPBO.JS.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Model.Validations
{
    public class FileWeightValidation : ValidationAttribute
    {
        private readonly int maxFileWeightInMb;
        
        private int MaxFileWeightInBytes => maxFileWeightInMb * 1024 * 1024;

        public FileWeightValidation(int maxFileWeightInMb)
        {
            this.maxFileWeightInMb = maxFileWeightInMb;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var formFile = value as IFormFile;
            if (formFile == null)
                return ValidationResult.Success;

            if (formFile.Length > MaxFileWeightInBytes)
                return new ValidationResult(string.Format(AppMessages.MaxFileWeightErrorMessage, $"{maxFileWeightInMb}MB"));

            return ValidationResult.Success;
        }
    }
}
