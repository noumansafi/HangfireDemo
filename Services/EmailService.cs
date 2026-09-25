namespace HangfireDemo.Services;

public class EmailService
{
    public void SendPaymentResult(
        string userEmail,
        bool successful)
    {
        var status = successful ? "SUCCESSFUL" : "FAILED";

        Console.WriteLine(
            $"Email sent to {userEmail}: Payment {status}");
    }
}