using FluentValidation;
using HomeApi.Contracts.Models.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Contracts.Validation
{
    public class EditRoomRequestValidator : AbstractValidator<EditRoomRequest>
    {
        public EditRoomRequestValidator()
        {
            // Проверяем площадь, только если она была передана
            RuleFor(x => x.NewArea)
                .GreaterThan(0)
                .When(x => x.NewArea.HasValue)
                .WithMessage("Площадь комнаты должна быть больше 0 кв. м.");

            // Проверяем напряжение, только если оно передано
            RuleFor(x => x.NewVoltage)
                .InclusiveBetween(100, 380)
                .When(x => x.NewVoltage.HasValue)
                .WithMessage("Напряжение должно быть в диапазоне от 100 до 380 В.");
        }
    }
}
