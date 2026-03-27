using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters  ;

namespace LoginAndRegistrationWebAPI.Filters
{
    public class ExceptionFilter:ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            // base.OnException(context);
            var ex=context.Exception;
            context.Result=new OkObjectResult(new {err=true,errDec=ex.Message});
        }
    }
}
