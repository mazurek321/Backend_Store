using projekt.src.Models.Users;

namespace projekt.src.Models.Store;

public class Announcement
{
    public Announcement(){}
    private Announcement(Guid id, UserId ownerId, Item item, DateTime createdAt)
    {
        Id = id;
        OwnerId = ownerId;
        Item = item;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set;}
    public UserId OwnerId{ get; private set;}
    public Item Item{get; private set;}
    public DateTime CreatedAt { get; private set;}

    public static Announcement New(UserId ownerId, Item item)
    {
        return new Announcement(Guid.NewGuid(), ownerId, item, DateTime.UtcNow);
    }
}