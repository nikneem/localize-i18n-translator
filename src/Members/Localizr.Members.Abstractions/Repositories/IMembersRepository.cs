using Localizr.Members.Abstractions.DomainModels;

namespace Localizr.Members.Abstractions.Repositories;

public interface IMembersRepository
{
    Task<IMember?> Get(string subjectId, CancellationToken cancellationToken);
    Task<bool> Save(IMember member, CancellationToken cancellationToken);
}