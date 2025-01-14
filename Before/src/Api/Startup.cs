﻿using System.Collections.Generic;

using Api.Utils;

using CSharpFunctionalExtensions;

using Logic.Commands;
using Logic.Dtos;
using Logic.Handlers;
using Logic.Options;
using Logic.Queries;
using Logic.Utils;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opts => opts.EnableEndpointRouting = false);

            services.AddSingleton<SessionFactory>();
            services.AddSingleton<Messages>();

            services.AddTransient<UnitOfWork>();
            services.AddTransient<ICommandHandler<DisenrollCommand>, DisenrollHandler>();
            services.AddTransient<ICommandHandler<EditPersonalInfoCommand>, EditPersonalInfoHandler>();
            services.AddTransient<ICommandHandler<EnrollCommand>, EnrollHandler>();
            services.AddTransient<ICommandHandler<RegisterStudentCommand>, RegisterStudentHandler>();
            services.AddTransient<ICommandHandler<TransferCommand>, TransferHandler>();
            services.AddTransient<ICommandHandler<UnregisterStudentCommand>, UnregisterStudentHandler>();

            services.AddTransient<IQueryHandler<GetStudentListQuery, Result<List<StudentDto>>>, GetStudentListHandler>();

            services.AddOptions<CommandDbOptions>().Bind(Configuration.GetSection(nameof(CommandDbOptions)));
            services.AddOptions<QueryDbOptions>().Bind(Configuration.GetSection(nameof(QueryDbOptions)));

            services.AddHandlers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }
    }
}
