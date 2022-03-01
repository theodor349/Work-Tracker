using DataAccess;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Shared;
using TimeSheetGeneration;
using WorkTracker;

var builder = WebApplication.CreateBuilder(args);

// CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyOrigin();
                          builder.AllowAnyMethod();
                          builder.AllowAnyHeader();
                      });
});

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IFileAccessSetup, FileAccessSetup>();
builder.Services.AddMediatR(typeof(WorkTrackerMediatREntrypoint).Assembly);
builder.Services.AddDataAccess();
builder.Services.AddMediatR(typeof(TimeSheetGenerationMediatREntrypoint).Assembly);
builder.Services.AddTimeSheetGeneration();


var app = builder.Build();
app.Urls.Clear();
app.Urls.Add("http://0.0.0.0:" + builder.Configuration["Hosting:Port"]);

// Configure the HTTP request pipeline.
if (bool.Parse(builder.Configuration["Hosting:UseSwagger"]))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if(bool.Parse(builder.Configuration["Hosting:UseHttps"]))
    app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
