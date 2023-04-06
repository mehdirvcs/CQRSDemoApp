using ApiDemo;
using ApiDemo.Controllers;
using DataAccess;
using DataAccess.DbAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing.Patterns;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterServicesToDI(builder.Configuration);

var serv=builder.Configuration.GetSection("MySetting").GetSection("Server").Value;
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "V2");

    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
//app.MapControllers();

app.Run();
