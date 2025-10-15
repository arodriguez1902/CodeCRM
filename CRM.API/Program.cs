using CRM.API.Extensions;
using CRM.Domain.Entities;
using CRM.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure Services
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

// CONFIGURACIÓN SIMPLIFICADA - Crear BD y datos semilla
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Crear BD y tablas si no existen (sin migraciones)
    context.Database.EnsureCreated();
    
    // Ejecutar datos semilla
    SeedData.Initialize(context);
}

// Configure Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();