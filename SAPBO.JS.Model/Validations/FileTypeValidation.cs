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
    public class FileTypeValidation : ValidationAttribute
    {
        private readonly string[] validFileTypes;

        public FileTypeValidation(string[] validFileTypes)
        {
            this.validFileTypes = validFileTypes;
        }

        public FileTypeValidation(Enums.FileTypeGroup fileTypeGroup)
        {
            if (fileTypeGroup == Enums.FileTypeGroup.Image)
                this.validFileTypes = new string[] { "image/jpeg", "image/png", "image/gif" };
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var formFile = value as IFormFile;
            if (formFile == null)
                return ValidationResult.Success;

            if (!validFileTypes.Contains(formFile.ContentType))
                return new ValidationResult(string.Format(AppMessages.ValidFileTypeErrorMessage, string.Join(", ", validFileTypes)));

            return ValidationResult.Success;
        }
    }
}
