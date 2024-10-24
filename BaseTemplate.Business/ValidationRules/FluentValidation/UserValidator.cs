﻿using BaseTemplate.Domain.Dtos.ExampleDtos;
using BaseTemplate.Shared.Dtos.LoginDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Business.ValidationRules.FluentValidation
{
    public class UserValidator : AbstractValidator<LoginRequestDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı Adı alanı boş olamaz.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre Alanı boş olamaz.").MinimumLength(3).WithMessage("Şifre en az 3 karakter olmalıdır.");
        }
    }
}