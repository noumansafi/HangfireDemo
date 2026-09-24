namespace HangfireDemo.Jobs;

public class EmailJob
{
    public void SendEmail()
    {
        Console.WriteLine(
            $"Email sent at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
    }
}