using Localizr.Core.Abstractions.Cqrs;

namespace Localizr.Members.Features.MemberGet;

public record MemberGetQuery(string SubjectId) : IQuery;

