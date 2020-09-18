using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManager.Application.Queries;
using TaskManager.DAL;
using TaskManager.Domain;
using TaskManager.Interfaces;
using TaskManager.Services;

namespace TaskManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(typeof(Startup));

            services.AddSingleton<ICloudTableFactory, CloudTableFactory>(options => new CloudTableFactory(Configuration.GetConnectionString("AzureDatabase")));
            services.AddSingleton<IGenericRepository<ProjectEntity>, GenericRepository<ProjectEntity>>();
            services.AddSingleton<IGenericRepository<TaskEntity>, GenericRepository<TaskEntity>>();
            services.AddSingleton<ITableQueries<ProjectEntity, ProjectViewModel>, TableQueries<ProjectEntity, ProjectViewModel>>();
            services.AddSingleton<ITableQueries<TaskEntity, TaskViewModel>, TableQueries<TaskEntity, TaskViewModel>>();
            services.AddSingleton<IProjectService, ProjectService>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
