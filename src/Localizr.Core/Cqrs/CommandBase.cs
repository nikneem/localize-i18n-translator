using Localizr.Core.Abstractions.Cqrs;

namespace Localizr.Core.Cqrs;

public record CommandBase : ICommand
{
    public Guid CommandId { get; init; } = Guid.NewGuid();
}