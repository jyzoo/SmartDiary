using Microsoft.EntityFrameworkCore;
using SmartDiary.Web.Data;
using SmartDiary.Web.Models;
using System.Diagnostics;
using DiaryTask = SmartDiary.Web.Models.Task;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

SeedData(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static void SeedData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.Migrate();

    if (context.Users.Any())
    {
        return;
    }

    var user = new User
    {
        Username = "testuser",
        Email = "test@example.com",
        PasswordHash = "test-password-hash",
        Settings = "{\"theme\":\"light\",\"notifications\":true}"
    };

    context.Users.Add(user);
    context.SaveChanges();

    var projects = new[]
    {
        new Project
        {
            Name = "Личные дела",
            Description = "Личные задачи",
            Color = "FF5733",
            OwnerId = user.Id
        },
        new Project
        {
            Name = "Работа",
            Description = "Рабочие задачи",
            Color = "33FF57",
            OwnerId = user.Id
        },
        new Project
        {
            Name = "Учеба",
            Description = "Учебные задачи",
            Color = "3357FF",
            OwnerId = user.Id
        }
    };

    context.Projects.AddRange(projects);
    context.SaveChanges();

    var tags = new[]
    {
        new Tag { Name = "Важное", OwnerId = user.Id },
        new Tag { Name = "Срочное", OwnerId = user.Id },
        new Tag { Name = "Идея", OwnerId = user.Id },
        new Tag { Name = "Личное", OwnerId = user.Id },
        new Tag { Name = "Рабочее", OwnerId = user.Id }
    };

    context.Tags.AddRange(tags);
    context.SaveChanges();

    var tasks = new[]
    {
        new DiaryTask
        {
            Title = "Купить продукты",
            Description = "Молоко, хлеб, яйца",
            Status = "New",
            Priority = "Medium",
            UserId = user.Id,
            ProjectId = projects[0].Id,
            Deadline = DateTime.UtcNow.AddDays(1)
        },
        new DiaryTask
        {
            Title = "Сдать отчет",
            Description = "Подготовить квартальный отчет",
            Status = "InProgress",
            Priority = "High",
            UserId = user.Id,
            ProjectId = projects[1].Id,
            Deadline = DateTime.UtcNow.AddHours(5)
        },
        new DiaryTask
        {
            Title = "Прочитать книгу",
            Description = "Глава 3",
            Status = "New",
            Priority = "Low",
            UserId = user.Id,
            ProjectId = projects[2].Id,
            Deadline = null
        },
        new DiaryTask
        {
            Title = "Позвонить маме",
            Description = "",
            Status = "New",
            Priority = "Medium",
            UserId = user.Id,
            ProjectId = null,
            Deadline = DateTime.UtcNow.AddDays(2)
        }
    };

    context.Tasks.AddRange(tasks);
    context.SaveChanges();

    var taskTags = new[]
    {
        new TaskTag { TaskId = tasks[0].Id, TagId = tags[1].Id },
        new TaskTag { TaskId = tasks[0].Id, TagId = tags[3].Id },
        new TaskTag { TaskId = tasks[1].Id, TagId = tags[0].Id },
        new TaskTag { TaskId = tasks[1].Id, TagId = tags[1].Id },
        new TaskTag { TaskId = tasks[1].Id, TagId = tags[4].Id },
        new TaskTag { TaskId = tasks[2].Id, TagId = tags[2].Id },
        new TaskTag { TaskId = tasks[3].Id, TagId = tags[3].Id }
    };

    context.TaskTags.AddRange(taskTags);
    context.SaveChanges();
}
