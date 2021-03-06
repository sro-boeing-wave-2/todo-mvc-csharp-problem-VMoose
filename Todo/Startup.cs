﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Todo.Models;
using Todo.service;
using Swashbuckle.AspNetCore.Swagger;
using System.Web.Http;

namespace Todo
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment currentEnvironment)
        {
            Configuration = configuration;
            _currentEnvironment = currentEnvironment;


        }
        public IHostingEnvironment _currentEnvironment { get; }
        public IConfiguration Configuration { get;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            if (_currentEnvironment.IsEnvironment("Testing"))
            {
                services.AddDbContext<TodoContext>(options =>
                      options.UseInMemoryDatabase("TodoContext"));
            }
            else
            {
                //services.AddDbContext<TodoContext>(options =>
                //      options.UseSqlServer(Configuration.GetConnectionString("TodoContext")));
                services.AddDbContext<TodoContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("TodoContext"), dbOptions => dbOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null)));

            }
            services.AddScoped<IServices, Service>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Google Keep API clone", Version = "v1" });
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env , TodoContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc();
            if (env.IsDevelopment())
            {
                context.Database.Migrate();
            }
            
        }
    }
}
