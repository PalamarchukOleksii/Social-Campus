﻿namespace Presentation.Endpoints.Users.Register;

public class RegisterRequest
{
    public string Login { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid VerifyEmailToken { get; set; } = Guid.Empty;
}