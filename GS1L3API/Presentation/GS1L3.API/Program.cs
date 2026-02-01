using GS1L3.Persistence;
using GS1L3.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();

//builder.Services.AddDbContext<GS1L3DbContext>(options =>
//            options.UseSqlServer(builder.Configuration.GetConnectionString("MssqlConnectionString")));

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("MssqlConnectionString"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true,
            BatchPostingLimit = 1
        })
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "GS1 L3 Serialization API",
        Version = "v1",
        Description = "Ýlaç ve Gýda serilizasyon süreçlerini yöneten L3 seviye API dokümantasyonu.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Hakan Certel",
            Email = "certel.hakan@gmail.com",

        }
    });

    // XML Yorumlarýný Swagger'a dahil et (Controller üzerindeki /// yorumlarý okur)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

try
{
    Log.Information("Uygulama baþlatýlýyor...");

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GS1 L3 API v1");
        c.RoutePrefix = string.Empty; // Swagger'ýn direkt ana sayfada (localhost:5000) açýlmasýný saðlar
    });
    }
    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Uygulama beklenmedik þekilde sonlandý");
}
finally
{
    Log.CloseAndFlush();
}
