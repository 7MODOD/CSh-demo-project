

using Microsoft.AspNetCore.Http;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.Configure<JsonSerializerOptions>(o => o.PropertyNameCaseInsensitive = true);

var app = builder.Build();





app.Use(async  (ctx, next) =>
{

    await next(ctx);
    /*if (context.Request.Headers.Authorization == string.Empty)
    {
        throw new Exception("You must be login to do this process\n");
    }
    else
    {
        await next(context);

    }*/
}


);

app.MapControllers();
app.Run("http://localhost:5500");