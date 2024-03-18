namespace GuessTheNumbers
{
    public class ConfigMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var gaId = configuration.GetValue<string>("GA_ID");
            context.Items["GA_ID"] = gaId;
    
            await next(context);
        }
    }
}
