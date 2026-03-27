using Microsoft.EntityFrameworkCore;
using Campus_Companion.Data;
using Campus_Companion.Models;
using Microsoft.AspNetCore.Http;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=campus-companion.db"));

builder.Services.AddDistributedMemoryCache();
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10 MB
});

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromHours(6);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseHttpsRedirection();
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Schedules.Any() && !db.Tasks.Any() && !db.Notices.Any())
    {
        db.Schedules.AddRange(
            new ScheduleItem { Title = "Math Lecture", Date = DateTime.Today, Time = "10:00 AM", Location = "Room 204", Category = "Lecture" },
            new ScheduleItem { Title = "Physics Lab", Date = DateTime.Today.AddDays(1), Time = "02:30 PM", Location = "Lab B", Category = "Event" }
        );

        db.Tasks.AddRange(
            new TaskItem { Title = "Database Assignment", Description = "Normalize schema and submit PDF.", DueDate = DateTime.Today.AddDays(2), Priority = TaskPriority.High, Status = ItemStatus.Pending },
            new TaskItem { Title = "Prepare API Notes", Description = "Review REST principles for exam.", DueDate = DateTime.Today.AddDays(5), Priority = TaskPriority.Medium, Status = ItemStatus.Pending }
        );

        db.Notices.AddRange(
            new NoticeItem { Title = "Exam Form Deadline", Content = "Submit exam form before Monday 5 PM.", Date = DateTime.Today, Category = "Exam" },
            new NoticeItem { Title = "Holiday Notice", Content = "Campus closed on Sunday.", Date = DateTime.Today.AddDays(-1), Category = "General" }
        );

        db.SaveChanges();
    }
}



app.MapControllerRoute(
    name: "student",
    pattern: "{controller=StudentDashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    
app.Run();
