using BlogApi.Models.Dtos;
using FluentValidation;

namespace BlogApi.Validations
{
    public class PostValidatorDto : AbstractValidator<PostDto>
    {
        public PostValidatorDto()
        {
            RuleFor(post => post.Title)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .Length(5, 100).WithMessage("El título debe tener entre 5 y 100 caracteres.");

            RuleFor(post => post.Content)
                .NotEmpty().WithMessage("El contenido es obligatorio.")
                .MinimumLength(10).WithMessage("El contenido debe tener al menos 10 caracteres.");

            RuleFor(post => post.Category)
                .NotEmpty().WithMessage("La categoría es obligatoria.");

            RuleFor(post => post.Tags)
                .NotNull().WithMessage("Las etiquetas no pueden ser nulas.")
                .Must(tags => tags.Count > 0).WithMessage("Debes proporcionar al menos una etiqueta.")
                .ForEach(tag => tag.NotEmpty().WithMessage("Las etiquetas no pueden estar vacías."));
        }
    }
}
