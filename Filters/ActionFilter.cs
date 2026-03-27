using Microsoft.AspNetCore.Mvc.Filters;

namespace LoginAndRegistrationWebAPI.Filters
{
    public class ActionFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
           // base.OnActionExecuting(context);
            Console.WriteLine("Before executing action method...");
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
          //  base.OnActionExecuted(context);
            Console.WriteLine("After executing action method...");
        }
    }
}
