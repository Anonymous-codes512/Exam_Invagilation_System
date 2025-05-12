using Exam_Invagilation_System.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();


// Configure the database context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
           .EnableSensitiveDataLogging();  // Enable sensitive data logging here
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutterApp",
        builder => builder
            .AllowAnyOrigin() // Later: use specific IP or domain
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// ✅ Properly resolve and use AppDbContext for database seeding
//using (var scope = app.Services.CreateScope())
//{
//    Console.WriteLine("Seeding database...");
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Fix
//    DBSeeder.SeedDatabase(dbContext);
//    Console.WriteLine("Database seeded successfully");
//}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseCors("AllowFlutterApp");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
