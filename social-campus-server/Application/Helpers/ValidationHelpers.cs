using Domain.Consts;

namespace Application.Helpers
{
    public static class ValidationHelpers
    {
        public static bool IsDomainValid(string email)
        {
            string[] allowedDomains = EmailDomains.AllowedDomains;
            string domain = email.Split('@')[1];

            return allowedDomains.Contains(domain);
        }
    }
}
