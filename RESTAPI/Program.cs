#region

using Identity;
using Microsoft.Extensions.FileProviders;
using Persistence;
using RESTAPI;

#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(config => {
    config.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddRESTAPIServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

app.UseCors();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, builder.Configuration["Application:ItemPhotosPath"])),
    RequestPath = builder.Configuration["Application:ItemPhotosPath"].Substring(1, builder.Configuration["Application:ItemPhotosPath"].Length - 2)
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();