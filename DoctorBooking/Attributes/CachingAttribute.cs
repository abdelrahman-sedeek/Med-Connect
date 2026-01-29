using Doctor_Booking.Application.Features.Caching.Commands;
using Doctor_Booking.Application.Features.Caching.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using System.Text.Json;

namespace DoctorBooking.Attributes
{
    public class CachingAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Create Cache Key
            string CacheKey = CreateCacheKey(context.HttpContext.Request);
            // Search For Value With Cacke Key
            IMediator mediator = context.HttpContext.RequestServices.GetRequiredService<IMediator>();
            GetWithStringKeyQuery getWithStringKeyQuery = new GetWithStringKeyQuery(CacheKey);
            var value = await mediator.Send(getWithStringKeyQuery);
            // Return Value if Not Null
            if (value is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = value,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            // If Null
            // Invoke Next
            var executedContext = await next.Invoke();
            // Set Value(Response) with Cache Key
            if (executedContext.Result is OkObjectResult result)
            {
                var value1 = JsonSerializer.Serialize(result);
                SetWithKeyCommand setWithKeyCommand = new SetWithKeyCommand(CacheKey, value1, TimeSpan.FromMinutes(4));
                await mediator.Send(setWithKeyCommand);
            }
            // Return Value
            return;

        }
        private string CreateCacheKey(HttpRequest httpRequest)
        {
            StringBuilder key = new StringBuilder();
            key.Append(httpRequest.Path);
            key.Append("?");
            foreach (var item in httpRequest.Query.OrderBy(Q => Q.Key))
            {
                key.Append($"{item.Key}={item.Value}&");
            }


            return key.ToString();
        }
    }
}
