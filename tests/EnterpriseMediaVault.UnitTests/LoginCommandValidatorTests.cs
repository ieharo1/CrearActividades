using EnterpriseMediaVault.Application.Validators;
using EnterpriseMediaVault.Application.Features.Auth.Commands;
using FluentAssertions;

namespace EnterpriseMediaVault.UnitTests;

public sealed class LoginCommandValidatorTests
{
    [Fact]
    public void Should_fail_when_email_is_invalid()
    {
        var validator = new LoginCommandValidator();
        var result = validator.Validate(new LoginCommand("not-an-email", "12345678"));
        result.IsValid.Should().BeFalse();
    }
}
