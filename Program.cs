using Exam_Invagilation_System.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Configure the database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));



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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
