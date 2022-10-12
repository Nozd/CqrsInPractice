using CSharpFunctionalExtensions;

using Logic.Commands;

namespace Logic.Handlers
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }
}
