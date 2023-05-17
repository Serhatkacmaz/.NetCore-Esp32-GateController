using GateController.API.Middlewares.WebsocketMiddleware;
using GateController.Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.File(@"Logs\App.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 90)
    .CreateLogger();


builder.Services.AddSingleton<SystemDevicesRepository>();
builder.Services.AddSingleton<SystemCardsRepository>();
builder.Services.AddSingleton<GateLogRepository>();
builder.Services.AddSingleton<OfficeMemberRepository>();
builder.Services.AddWSServerConnectionManager();

// Add services to the container.
builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseWebSocketServer();

app.UseAuthorization();


app.MapControllers();

app.Run();
