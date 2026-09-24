using Hangfire;
using Hangfire.SqlServer;
using HangfireDemo.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("HangfireConnection")));

builder.Services.AddHangfireServer();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire");

// Register recurring job AFTER the application is built
var recurringJobManager = app.Services.GetRequiredService<IRecurringJobManager>();

recurringJobManager.AddOrUpdate<EmailJob>(
    "daily-email-job",
    job => job.SendEmail(),
    Cron.Minutely);

app.MapGet("/", () => "Hangfire Demo is running!");

app.MapPost("/jobs/send-email", () =>
{
    var jobId = BackgroundJob.Enqueue<EmailJob>(
        job => job.SendEmail());

    return Results.Ok(new
    {
        Message = "Email job queued",
        JobId = jobId
    });
});

app.MapPost("/jobs/send-email-delayed", () =>
{
    var jobId = BackgroundJob.Schedule<EmailJob>(
        job => job.SendEmail(),
        TimeSpan.FromMinutes(1));

    return Results.Ok(new
    {
        Message = "Email job scheduled for 1 minute from now",
        JobId = jobId
    });
});

app.Run();