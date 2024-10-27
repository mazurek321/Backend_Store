using projekt.src.Models.Users;

namespace projekt.src.Models.SavedAnnouncements;

public class SavedAnnouncements
{
    private SavedAnnouncements(UserId userId, Guid announcementId){
        UserId = userId;
        AnnouncementId = announcementId;
    }

    public UserId UserId { get; private set; }
    public Guid AnnouncementId { get; private set; }

    public static SavedAnnouncements SaveAnnouncement(UserId userId, Guid announcementId)
    {
        return new SavedAnnouncements(userId, announcementId);
    }
}
