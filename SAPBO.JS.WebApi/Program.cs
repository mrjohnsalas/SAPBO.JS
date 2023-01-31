using SAPBO.JS.Common;
using SAPBO.JS.Model.App;
using SAPBO.JS.WebApi;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();
