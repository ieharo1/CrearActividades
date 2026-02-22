namespace EnterpriseMediaVault.Application.Abstractions;

public interface IStorageStrategyResolver
{
    IFileStorageStrategy Resolve(string strategyName);
}
