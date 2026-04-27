namespace LoginAndRegistrationWebAPI.Extensions
{
    public static class EmailValidationExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            return !string.IsNullOrEmpty(email) && email.Contains("@") && email.Contains(".");
        }
    }
}
