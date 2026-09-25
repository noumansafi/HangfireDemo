using Hangfire;
using Hangfire.SqlServer;
using HangfireDemo.Data;
using HangfireDemo.Jobs;
using HangfireDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Application dependencies
builder.Services.AddSingleton<SubscriptionStore>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<EmailService>();

// Hangfire stores jobs, schedules, states, retries, etc. in SQL Server.
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("HangfireConnection")));

// Starts Hangfire background workers.
builder.Services.AddHangfireServer();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire");

// Recurring job definition is stored in Hangfire's persistent storage.
// Hangfire will create an execution every minute according to this schedule.
var recurringJobManager = app.Services
    .GetRequiredService<IRecurringJobManager>();

recurringJobManager.AddOrUpdate<SubscriptionPaymentJob>(
    "subscription-payment-job",
    job => job.ProcessPayments(),
    Cron.Minutely);

app.MapGet("/", () => "Hangfire Demo is running!");

// Fire-and-forget job.
// The API request only queues the job; Hangfire executes it in the background.
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

// Delayed job.
// The job is persisted immediately but won't be executed until the scheduled time.
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