﻿using Domain.Models.FollowModel;
using Domain.Models.PublicationModel;
using Domain.Models.RefreshTokenModel;

namespace Domain.Models.UserModel
{
    public class User
    {
        private User() { }

        public User(
            string login,
            string passwordHash,
            string email,
            string firstName,
            string lastName)
        {
            Id = new UserId(Guid.NewGuid());
            Login = login;
            PasswordHash = passwordHash;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public UserId Id { get; private set; } = new UserId(Guid.Empty);
        public string Login { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Bio { get; private set; } = string.Empty;
        public string ProfileImageData { get; private set; } = string.Empty;
        public RefreshTokenId RefreshTokenId { get; private set; } = new RefreshTokenId(Guid.Empty);
        public virtual RefreshToken? RefreshToken { get; }
        public virtual ICollection<Follow>? Followers { get; }
        public virtual ICollection<Follow>? FollowedUsers { get; }
        public virtual ICollection<Publication>? Publications { get; }

        public void SetRefreshTokenId(RefreshTokenId refreshTokenId) => RefreshTokenId = refreshTokenId;

        public void DropRefreshTokenIdOnRevoke() => RefreshTokenId = new RefreshTokenId(Guid.Empty);

        public void Update(string login, string email, string firstName, string lastName, string bio, string profileImageData)
        {
            Login = login;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Bio = bio;
            ProfileImageData = profileImageData;
        }
    }
}
