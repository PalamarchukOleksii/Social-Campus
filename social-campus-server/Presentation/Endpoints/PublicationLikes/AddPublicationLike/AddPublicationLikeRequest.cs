﻿using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Presentation.Endpoints.PublicationLikes.AddPublicationLike;

public class AddPublicationLikeRequest
{
    public UserId UserId { get; set; } = new(Guid.Empty);
    public PublicationId PublicationId { get; set; } = new(Guid.Empty);
}