using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Winton.Extensions.Configuration.Consul;

namespace sample.consul.api.Consul
{
    static class ConfigBuilderExtension
    {
        public const string ConsulHostSection = "Consul:Host";
        public const string ConsulAppSection = "Consul:AppName";

        public static IConfigurationBuilder ConsulUseAppConfiguration(this IConfigurationBuilder builder, HostBuilderContext ctx, string[] args)
        {
            var env = ctx.HostingEnvironment;

            var cfg = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
                .AddEnvironmentVariables()
                .Build();

            var consulHost = cfg.GetSection(ConsulHostSection).Value;
            var consulApp = cfg.GetSection(ConsulAppSection).Value;

            builder.AddConsul($"{consulApp}/appsettings.json",
                options =>
                {
                    options.ConsulConfigurationOptions =
                    cco => { cco.Address = new Uri(consulHost); };
                    options.Optional = true;
                    options.ReloadOnChange = true;
                    options.OnLoadException = exceptionContext => { exceptionContext.Ignore = true; };
                    options.PollWaitTime = TimeSpan.FromSeconds(1);
                }
            )
            .AddEnvironmentVariables()
            .AddCommandLine(args);
            ;

            return builder;
        }
    }
}
