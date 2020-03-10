using System;
using Circa.Actors.Application;
using Circa.Actors.Domain;
using Circa.Actors.Domain.Operations;
using Circa.Actors.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Solari.Callisto;
using Solari.Callisto.Connector;
using Solari.Callisto.Tracer;
using Solari.Deimos;
using Solari.Sol;
using Solari.Titan.DependencyInjection;
using Solari.Vanth.DependencyInjection;

namespace Circa.Actors.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) { Configuration = configuration; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // IConveyBuilder builder = ConveyBuilder.Create(services);
            services.AddControllers();
            // builder.AddRabbitMq()
            //        .AddCommandHandlers()
            //        .AddQueryHandlers()
            //        .AddEventHandlers()
            //        .AddServiceBusCommandDispatcher()
            //        .AddServiceBusEventDispatcher();

            services.AddScoped<PersonOperations>();
            services.AddScoped<IPersonApplication, PersonApplication>();
            services.AddSol(Configuration)
                    .AddCallistoConnector()
                    .AddDeimos(manager => manager.Register(new CallistoTracerPlugin()))
                    .AddCallisto(a => a.RegisterDefaultClassMaps()
                                       .RegisterDefaultConventionPack()
                                       .RegisterCollection<IPersonRepository, PersonRepository, Person>("person", ServiceLifetime.Scoped))
                    .AddTitan()
                    .AddVanth();
                  
            services.AddSwaggerGen(a => a.SwaggerDoc("v1", new OpenApiInfo
            {
                Contact = new OpenApiContact
                {
                    Email = "luccas.lauth@gmail.com",
                    Name = "Luccas Lauth Gianesini"
                },
                Description = "Actors manages users and it's profiles;",
                License = new OpenApiLicense
                {
                    Name = "GNU General Public License v3.0",
                    Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.en.html")
                },
                Title = "Circa Actors",
                Version = "v1",
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseConvey();
            app.UseSol();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Circa Actors");
            });    

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}