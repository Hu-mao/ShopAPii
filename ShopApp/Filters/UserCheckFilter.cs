using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Domain.Models;

namespace ShopApp.Filters
{
    public class UserCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.ActionArguments.TryGetValue("user", out var value)
                && value is User user)
            {
                if (user.Login == "admin" && user.Id == 1)
                {
                
                    base.OnActionExecuting(context);
                    return;
                }
            }


            context.Result = new UnauthorizedObjectResult(new
            {
                message = "No authorization"
            });
        }
    }
}