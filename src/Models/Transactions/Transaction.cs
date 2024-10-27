// using projekt.src.Models.Users;

// namespace projekt.src.Models.Transactions;

// public class Transaction
// {
//     private Transaction(Guid id, UserId userId, Guid orderId, DateTime occuredOn, bool payed)
//     {
//         Id = id;
//         UserId = userId;
//         OrderId = orderId;
//         OccuredOn = occuredOn;
//         Payed = payed;
//     }

//     public Guid Id { get; private set; }
//     public UserId UserId { get; private set; }
//     public Guid OrderId { get; private set; }
//     public DateTime OccuredOn { get; private set; }
//     public bool Payed { get; private set; }

//     public static Transaction New(UserId userId, Guid orderId, DateTime occuredOn, bool payed)
//     {
//         return new Transaction(Guid.NewGuid(), userId, orderId, occuredOn, payed);
//     }
// }