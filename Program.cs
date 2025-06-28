using ProductosAPI.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext para PostgreSQL
builder.Services.AddDbContext<ProductosContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

// Asegúrate de que se apliquen las migraciones al arrancar (opcional)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductosContext>();
    db.Database.Migrate(); // Aplica migraciones si existen
}

app.MapControllers();

app.Run();
