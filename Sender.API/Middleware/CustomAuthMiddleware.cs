using Sender.Domain;

public class CustomAuthMiddleware
{
    private readonly RequestDelegate _next;

    public CustomAuthMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring(7);

            var decodedData = tokenService.DecodeToken<Token>(token);

            context.Items["Token"] = decodedData;
        }

        await _next(context);
    }
}