using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServicePlatform.Interface
{
    public class ValidateAntiForgeryHeader : IActionFilter
    {
        private readonly IAntiforgery _antiforgery;

        public ValidateAntiForgeryHeader(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Verify the anti-forgery token on incoming requests
            if (context.HttpContext.Request.Method == "POST")
            {
                _antiforgery.ValidateRequestAsync(context.HttpContext).Wait();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after the action is executed
        }

    }
}
