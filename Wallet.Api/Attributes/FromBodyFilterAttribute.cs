using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Wallet.Common.Codes;
using Wallet.Common.ExceptionHandling;

namespace Wallet.Api.Attributes
{
    public sealed class FromBodyFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IEnumerable<ControllerParameterDescriptor> fromBodyActionParameters = context.ActionDescriptor.Parameters.OfType<ControllerParameterDescriptor>()
                                                                                         .Where(p => p.ParameterInfo.GetCustomAttributes(typeof(FromBodyAttribute), false).Length > 0);

            if (fromBodyActionParameters.Any(p => !context.ActionArguments.ContainsKey(p.Name)))
            {
                throw new WalletException(ErrorCode.GenericError, "Invalid Request", System.Net.HttpStatusCode.BadRequest);
            }

            base.OnActionExecuting(context);
        }
    }
}
