using Microsoft.EntityFrameworkCore;
using ProductosAPI.Data;
//Hola
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddDbContext<ProductosContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine("🔌 Usando cadena de conexión: " + connectionString);


    builder.Services.AddControllers();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<ProductosContext>();
            db.Database.Migrate();
            Console.WriteLine("✅ Migraciones aplicadas correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Error al aplicar migraciones: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }

    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    app.Urls.Add($"http://*:{port}");

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("❌ FATAL ERROR: " + ex.Message);
    Console.WriteLine(ex.StackTrace);
}
