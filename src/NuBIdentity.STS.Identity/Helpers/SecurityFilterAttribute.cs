using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using NuBIdentity.STS.Identity.Configuration.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuBIdentity.STS.Identity.Helpers
{
    public class SecurityFilterAttribute : IActionFilter
    {
        private readonly IConfiguration _configuration;

        public SecurityFilterAttribute(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey(ConfigurationConsts.AccountApiKey))
            {
                context.Result = new UnauthorizedObjectResult("Api Key not found");
                return;
            }

            if (context.HttpContext.Request.Headers[ConfigurationConsts.AccountApiKey] != _configuration["AccountApiSecurity:ApiKey"])
            {
                context.Result = new UnauthorizedObjectResult("Invalid Api Key");
            }
        }

    }
}
