using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json.Converters;
using Wallet.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var env = builder.Environment; 

services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);

services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressConsumesConstraintForFormFileParameters = true;
        options.SuppressInferBindingSourcesForParameters = false;
        options.SuppressModelStateInvalidFilter = true;
        options.SuppressMapClientErrors = true;
        options.ClientErrorMapping[404].Link =
            "https://httpstatuses.com/404";
    });  
 

services.AddWalletServices(builder.Configuration);

services.AddControllers();

services.AddSwagger(env);

services.AddMvc(options => options.EnableEndpointRouting = false)
    .AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

var app = builder.Build();
if (env.IsEnvironment("dev"))
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseSwaggerWithUi(env);

app.UseWalletMiddlewares();

app.UseAuthorization();
app.MapControllers();
app.Run();