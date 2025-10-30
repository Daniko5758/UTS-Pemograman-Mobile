using Microsoft.EntityFrameworkCore;
using MyTraining.Backend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use InMemory DB for simplicity
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("TrainingDb"));

// Allow BLazor app to call backend (adjust origins if needed)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();
app.MapControllers();

// Seed sample data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Trainers.Any())
    {
        var t1 = new MyTraining.Backend.Models.Trainer { FullName = "Budi Santoso", Expertise = "Web Development", Email = "budi@example.com" };
        var t2 = new MyTraining.Backend.Models.Trainer { FullName = "Siti Aminah", Expertise = "Data Science", Email = "siti@example.com" };
        db.Trainers.AddRange(t1, t2);
        db.Courses.AddRange(
            new MyTraining.Backend.Models.Course { Title = "ASP.NET Core Basics", Description = "Learn ASP.NET Core", Trainer = t1, DurationMinutes = 180, Price = 50 },
            new MyTraining.Backend.Models.Course { Title = "Intro to Python for Data", Description = "Data Science intro", Trainer = t2, DurationMinutes = 240, Price = 75 }
        );
        db.SaveChanges();
    }
}

app.Run();
