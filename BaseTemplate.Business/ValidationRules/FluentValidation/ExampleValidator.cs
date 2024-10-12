using BaseTemplate.Domain.Dtos.ExampleDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.ValidationRules.FluentValidation
{
    public class ExampleValidator : AbstractValidator<CreateExampleRequest>
    {
        public ExampleValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("İsim alanı boş geçilemez");
            RuleFor(x => x.Description).NotEmpty().MinimumLength(50).WithMessage("Açıklama alanı boş geçilemez");
        }
    }
}
