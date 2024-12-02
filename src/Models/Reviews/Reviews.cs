using projekt.src.Models.Users;

namespace projekt.src.Models.Reviews;

public class Reviews
{
    public Reviews(){}
    private Reviews(Guid id, Guid announcementId, UserId ownerId, string? comment, Rating rating, DateTime createdAt)
    {
        Id = id;
        AnnouncementId = announcementId;
        OwnerId = ownerId;
        Comment = comment;
        Rating = rating;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public Guid AnnouncementId { get; private set; } 
    public UserId OwnerId { get; private set; }
    public string? Comment {get; private set; }
    public Rating Rating { get; private set; }
    public DateTime CreatedAt {get; private set; }

    public static Reviews NewReview(Guid announcementId, UserId ownerId, string? comment, Rating rating)
    {
        return new Reviews(Guid.NewGuid(), announcementId, ownerId, comment, rating, DateTime.UtcNow);
    }
}