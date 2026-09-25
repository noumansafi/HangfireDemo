namespace HangfireDemo.Services;

public class PaymentService
{
    private readonly HashSet<Guid> _processedPayments = [];

    public bool ProcessPayment(
        Guid paymentId,
        decimal amount,
        string userEmail)
    {
        if (_processedPayments.Contains(paymentId))
        {
            Console.WriteLine(
                $"Payment {paymentId} was already processed. Skipping duplicate charge.");

            return true;
        }

        Console.WriteLine(
            $"Charging ${amount} from {userEmail}. PaymentId: {paymentId}");

        // Temporary failure simulation for testing Hangfire retries.
        // throw new Exception(
        //     "Payment gateway temporarily unavailable.");

        // Simulate successful payment.
        _processedPayments.Add(paymentId);

        Console.WriteLine(
            $"Payment {paymentId} processed successfully.");

        return true;
    }
}