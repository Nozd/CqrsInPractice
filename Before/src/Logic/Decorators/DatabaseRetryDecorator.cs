using System;

using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Handlers;
using Logic.Options;

using Microsoft.Extensions.Options;

namespace Logic.Decorators
{
    public sealed class DatabaseRetryDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly DbOptions _dbOptions;

        public DatabaseRetryDecorator(ICommandHandler<TCommand> handler, IOptions<DbOptions> dbOptions)
        {
            _dbOptions = dbOptions.Value;
            _handler = handler;
        }

        public Result Handle(TCommand command)
        {
            for (int i = 0; ; i++)
            {
                try
                {
                    Result result = _handler.Handle(command);
                    return result;
                }
                catch (Exception ex)
                {
                    if (i >= _dbOptions.NumberOfDatabaseRetries || !IsDatabaseException(ex))
                        throw;
                }
            }
        }

        private bool IsDatabaseException(Exception exception)
        {
            string message = exception.InnerException?.Message;

            if (message == null)
                return false;

            return message.Contains("The connection is broken and recovery is not possible")
                || message.Contains("error occurred while establishing a connection");
        }
    }
}
