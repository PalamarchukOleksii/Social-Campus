namespace Domain.Entities
{
    public class Follow
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }

        public int FollowedUserId { get; set; }
        public virtual User? FollowedUser { get; set; }
    }
}