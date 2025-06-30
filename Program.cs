using Microsoft.EntityFrameworkCore;
using ProductosAPI.Data;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
        ?? builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<ProductosContext>(options =>
        options.UseNpgsql(connectionString));

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
            Exception? current = ex;
            int level = 0;
            while (current != null)
            {
                Console.WriteLine($"❌ ERROR (nivel {level}): {current.GetType().FullName}: {current.Message}");
                Console.WriteLine(current.StackTrace);
                current = current.InnerException;
                level++;
            }
        }
    }

    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Urls.Add($"http://*:{port}");

    // Desactiva HTTPS en producción para evitar redirecciones 502
    // app.UseHttpsRedirection();

    app.UseAuthorization();
    app.MapControllers();

    // Mueve estos logs aquí antes de Run()
    Console.WriteLine($"📣 La aplicación se iniciará en el puerto: {port}");
    Console.WriteLine("DATABASE_URL: " + Environment.GetEnvironmentVariable("DATABASE_URL"));
    Console.WriteLine("PGHOST: " + Environment.GetEnvironmentVariable("PGHOST"));
    Console.WriteLine("PGUSER: " + Environment.GetEnvironmentVariable("PGUSER"));
    Console.WriteLine("PGPORT: " + Environment.GetEnvironmentVariable("PGPORT"));
    Console.WriteLine("PGDATABASE: " + Environment.GetEnvironmentVariable("PGDATABASE"));

    app.Run();
}
catch (Exception ex)
{
    Exception? current = ex;
    int level = 0;
    while (current != null)
    {
        Console.WriteLine($"❌ ERROR (nivel {level}): {current.GetType().FullName}: {current.Message}");
        Console.WriteLine(current.StackTrace);
        current = current.InnerException;
        level++;
    }
}
