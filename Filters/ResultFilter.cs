using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace LoginAndRegistrationWebAPI.Filters
{
    public class ResultFilter:ResultFilterAttribute
    {
        pubilc override void OnResultExecuting(ResultExecutingContext context)
        {
            // base.OnResultExecuting(context);
            Console.WriteLine("Before executing action result...");
            if(context.Result is ObjectResult objectResult)
            {
               objectResult.Value=new {
                   data=objectResult.Value,
                   err=false,
                   info="This is result filter"
               };
                
               
            }
        }
        public override void OnResultExecuted(ResultExecutedContext context)
            {
                // base.OnResultExecuted(context);
                Console.WriteLine("After executing action result...");
        }
    }
}
