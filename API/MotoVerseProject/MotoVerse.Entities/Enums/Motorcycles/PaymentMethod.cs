namespace MotoVerse.Entities.Enums.Motorcycles;


public enum PaymentMethod
{
    Cash = 1,
    Card = 2,
    Wallet = 3
}
public enum PaymentProvider
{
    None = 0,

    Visa = 1,

    Stripe = 2,

    Paypal = 3,

    GooglePay = 4
}