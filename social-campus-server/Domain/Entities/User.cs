﻿namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public int RefreshTokenId { get; set; }
        public virtual RefreshToken? RefreshToken { get; set; }

        public virtual ICollection<Follow>? Followers { get; set; }
        public virtual ICollection<Follow>? FollowedUsers { get; set; }
    }
}
