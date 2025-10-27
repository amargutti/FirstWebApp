using FirstWebApp.Models.EF_Models;
using FirstWebApp.Models.Services.Application;
using FirstWebApp.Models.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;

// Add CourseService to the DI container

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddTransient<ICourseService, AdoNetCourseService>();
builder.Services.AddTransient<ICourseService, EfCoreCourseService>();
builder.Services.AddTransient<IDatabaseAccessor, SqlServerAccessor>();

builder.Services.AddDbContextPool<FirstWebAppDBContext>(optionsBuilder =>
        optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\FirstWebApp;Database=FirstWebAppDB;Trusted_Connection=True;")
);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
// Middleware to serve static files that are in the wwwroot folder
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map controller route for CoursesController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

//Forma Abbreviata di router di defualt
//app.MapDefaultControllerRoute();

app.MapRazorPages();

app.Run();
 