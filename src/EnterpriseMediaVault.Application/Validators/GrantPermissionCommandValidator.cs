using EnterpriseMediaVault.Application.Features.Permissions.Commands;
using FluentValidation;

namespace EnterpriseMediaVault.Application.Validators;

public sealed class GrantPermissionCommandValidator : AbstractValidator<GrantPermissionCommand>
{
    public GrantPermissionCommandValidator()
    {
        RuleFor(x => x.SubjectId).NotEmpty();
        RuleFor(x => x.SubjectType).NotEmpty();
        RuleFor(x => x.ResourceId).NotEmpty();
        RuleFor(x => x.ResourceType).NotEmpty();
        RuleFor(x => x.Action).NotEmpty();
    }
}
