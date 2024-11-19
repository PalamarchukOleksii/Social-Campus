﻿namespace Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpiryTime { get; set; }

        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}