namespace HangfireDemo.Models;

public class Subscription
{
    public int Id { get; set; }

    public string UserEmail { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime NextPaymentDate { get; set; }

    public bool IsActive { get; set; }

    // Unique identifier for the payment operation.
    // This allows the payment service to recognize a retry
    // of the same payment and avoid charging twice.
    public Guid PaymentId { get; set; }
}