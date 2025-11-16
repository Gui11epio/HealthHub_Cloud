using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace Health_Hub.Application.Config
{
    public class SwaggerSettings
    {
        public string Title { get; init; }

        public string Description { get; init; }

        public OpenApiContact Contact { get; init; }
    }
}
