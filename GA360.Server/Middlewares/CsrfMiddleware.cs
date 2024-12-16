namespace GA360.Server.Middlewares;

public class CsrfMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _csrfTokenHeader = "X-CSRF-TOKEN";

    public CsrfMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Post)
        {
            var csrfToken = context.Request.Headers[_csrfTokenHeader].FirstOrDefault();
            if (string.IsNullOrEmpty(csrfToken) || !ValidateCsrfToken(context, csrfToken))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("CSRF token missing or invalid.");
                return;
            }
        }

        await _next(context);
    }

    private bool ValidateCsrfToken(HttpContext context, string token)
    {
        // Implement your token validation logic here
        var validToken = context.Session.GetString("CSRF-TOKEN");
        return validToken == token;
    }
}
