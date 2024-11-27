﻿using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Application.Dtos
{
    public class ShortPublicationDto
    {
        public PublicationId Id { get; set; } = new PublicationId(Guid.Empty);
        public string Description { get; set; } = string.Empty;
        public string ImageData { get; set; } = string.Empty;
        public DateTime CreationDateTime { get; set; }
        public IReadOnlyList<UserId>? UserWhoLikedIds { get; set; }
    }
}
