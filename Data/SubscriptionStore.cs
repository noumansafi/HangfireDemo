using HangfireDemo.Models;

namespace HangfireDemo.Data;

public class SubscriptionStore
{
    public List<Subscription> Subscriptions { get; } =
    [
        new Subscription
        {
            Id = 1,
            UserEmail = "user1@test.com",
            Amount = 20,
            NextPaymentDate = DateTime.Now,
            IsActive = true,

            // Represents the current payment operation.
            PaymentId = Guid.NewGuid()
        },

        new Subscription
        {
            Id = 2,
            UserEmail = "user2@test.com",
            Amount = 50,
            NextPaymentDate = DateTime.Now.AddDays(1),
            IsActive = true,

            PaymentId = Guid.NewGuid()
        },

        new Subscription
        {
            Id = 3,
            UserEmail = "user3@test.com",
            Amount = 30,
            NextPaymentDate = DateTime.Now,
            IsActive = false,

            PaymentId = Guid.NewGuid()
        }
    ];
}