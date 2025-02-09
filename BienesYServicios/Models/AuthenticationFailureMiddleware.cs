public class AuthenticationFailureMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationFailureMiddleware> _logger;

    public AuthenticationFailureMiddleware(RequestDelegate next, ILogger<AuthenticationFailureMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // No aplicar el middleware si ya estamos en la página de login
        if (context.Request.Path.StartsWithSegments("/Login/Index", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        try
        {
            await _next(context);

            if (context.Response.StatusCode == 401)
            {
                // Limpiar la cookie antes de redirigir
                context.Response.Cookies.Delete("SesionId");

                // Redirigir al login
                context.Response.Redirect("/Login/Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante la autenticación");
            context.Response.Cookies.Delete("SesionId");
            context.Response.Redirect("/Login/Index");
        }
    }
}