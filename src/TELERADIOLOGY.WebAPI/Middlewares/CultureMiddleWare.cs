using System.Globalization;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var langCode = context.Request.Headers["Lang"].FirstOrDefault();

        
        var culture = langCode switch
        {
            "1" => "tr",
            "2" => "en",
            _ => "tr" 
        };

        var cultureInfo = new CultureInfo(culture);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        await _next(context);
    }
}
