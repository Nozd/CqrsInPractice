﻿using System;

using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Handlers;
using Logic.Queries;

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

        public TResult Dispatch<TResult>(IQuery<TResult> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(TResult) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _serviceProvider.GetService(handlerType);
            TResult result = handler.Handle((dynamic)query);

            return result;
        }
    }
}
