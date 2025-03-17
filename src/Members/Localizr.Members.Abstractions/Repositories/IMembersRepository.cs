using Localizr.Members.Abstractions.DomainModels;

namespace Localizr.Members.Abstractions.Repositories;

public interface IMembersRepository
{
    Task<bool> Save(IMember member, CancellationToken cancellationToken);
}