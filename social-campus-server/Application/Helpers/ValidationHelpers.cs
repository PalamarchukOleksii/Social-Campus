using Domain.Consts;

namespace Application.Helpers
{
    public static class ValidationHelpers
    {
        public static bool IsDomainValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var parts = email.Split('@');
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[1]))
                return false;

            string[] allowedDomains = EmailDomains.AllowedDomains;

            string domain = parts[1];
            return allowedDomains.Contains(domain);
        }
    }
}
