using Hangfire;
using HangfireDemo.Data;
using HangfireDemo.Services;

namespace HangfireDemo.Jobs;

// If the payment gateway temporarily fails,
// Hangfire will retry the job up to 3 times.
[AutomaticRetry(Attempts = 3)]
public class SubscriptionPaymentJob(
    SubscriptionStore subscriptionStore,
    PaymentService paymentService,
    EmailService emailService)
{
    public void ProcessPayments()
    {
        // Find subscriptions whose next payment date has arrived.
        var dueSubscriptions = subscriptionStore.Subscriptions
            .Where(x =>
                x.IsActive &&
                x.NextPaymentDate <= DateTime.Now)
            .ToList();

        foreach (var subscription in dueSubscriptions)
        {
            var successful = paymentService.ProcessPayment(
                subscription.PaymentId,
                subscription.Amount,
                subscription.UserEmail);

            emailService.SendPaymentResult(
                subscription.UserEmail,
                successful);

            if (successful)
            {
                // Payment completed.
                // Move the subscription to its next billing date.
                subscription.NextPaymentDate =
                    subscription.NextPaymentDate.AddMonths(1);

                // Generate a new payment ID for the next billing cycle.
                //
                // The current PaymentId belongs only to the payment
                // that we just processed.
                subscription.PaymentId = Guid.NewGuid();
            }
        }
    }
}