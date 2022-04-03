﻿using AssessmentWebDev.utils;
using Microsoft.Extensions.DependencyInjection;

namespace AssessmentWebDev;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        StaticConfiguration = configuration;
    }

    public IConfiguration Configuration { get; }

    public static IConfiguration StaticConfiguration { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSession();
        services.AddAuthentication(Auth.AuthenticationSchemeName).AddCookie(Auth.AuthenticationSchemeName, options =>
        {
            options.Cookie.Name = Auth.AuthenticationSchemeName;
        });
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromHours(3);
        });
        services.AddMemoryCache();
        services.AddRazorPages();
        services.AddRouting(options =>
        {
                
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
}