using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.tools
{
    public static class OptionExtensions
    {
        public static TOptions GetToption<TOptions>(this IConfiguration configuration, string name) where TOptions: class, new()
        {
            var options = new TOptions();
            configuration.GetSection(name).Bind(options);

            return options;

        }
    }
}
