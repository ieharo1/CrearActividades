using EnterpriseMediaVault.Application.Features.Folders.Commands;
using FluentValidation;

namespace EnterpriseMediaVault.Application.Validators;

public sealed class CreateFolderCommandValidator : AbstractValidator<CreateFolderCommand>
{
    public CreateFolderCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
    }
}
