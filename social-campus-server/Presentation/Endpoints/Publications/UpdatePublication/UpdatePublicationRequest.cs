﻿using Domain.Models.PublicationModel;
using Domain.Models.UserModel;

namespace Presentation.Endpoints.Publications.UpdatePublication;

public class UpdatePublicationRequest
{
    public UserId CallerId { get; set; } = new(Guid.Empty);
    public PublicationId PublicationId { get; set; } = new(Guid.Empty);
    public string Description { get; set; } = string.Empty;
    public IFormFile? ImageData { get; set; }
}