﻿namespace Presentation.Dtos
{
    public class RefreshDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}