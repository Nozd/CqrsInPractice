using CSharpFunctionalExtensions;
using Logic.Students;
using System;

namespace Logic.Utils
{
    public sealed class Messages
    {
        private readonly IServiceProvider _serviceProvider;

        public Messages(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Result Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _serviceProvider.GetService(handlerType);
            Result result = handler.Handle((dynamic)command);

            return result;
        }
    }
}
