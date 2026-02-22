using EnterpriseMediaVault.Application.Features.Files.Commands;
using FluentValidation;

namespace EnterpriseMediaVault.Application.Validators;

public sealed class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.MimeType).NotEmpty();
        RuleFor(x => x.Size).GreaterThan(0);
        RuleFor(x => x.FolderId).NotEmpty();
        RuleFor(x => x.FileStream).NotNull();
    }
}
