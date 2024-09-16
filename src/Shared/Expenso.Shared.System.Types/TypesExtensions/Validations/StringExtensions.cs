using System.Net.Mail;

namespace Expenso.Shared.System.Types.TypesExtensions.Validations;

public static class StringExtensions
{
    public static bool IsValidEmail(this string? email)
    {
        if (string.IsNullOrWhiteSpace(value: email))
        {
            return false;
        }

        try
        {
            MailAddress mailAddress = new(address: email);

            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidHost(this string? host)
    {
        if (string.IsNullOrWhiteSpace(value: host))
        {
            return false;
        }

        UriHostNameType hostType = Uri.CheckHostName(name: host);

        return hostType is UriHostNameType.Dns or UriHostNameType.IPv4 or UriHostNameType.IPv6;
    }

    public static bool IsValidPassword(this string password, int minLength = 8, int maxLength = 128)
    {
        if (password.Length < minLength || password.Length > maxLength)
        {
            return false;
        }

        bool hasUppercase = password.Any(predicate: char.IsUpper);
        bool hasLowercase = password.Any(predicate: char.IsLower);
        bool hasDigit = password.Any(predicate: char.IsDigit);
        bool hasSpecialChar = password.Any(predicate: "!@#$%^&*()_+[]{}|;:,.<>?".Contains);

        return hasUppercase && hasLowercase && hasDigit && hasSpecialChar && !password.Contains(value: ' ');
    }

    public static bool IsValidUsername(this string username, int minLength = 3, int maxLength = 30)
    {
        if (username.Length < minLength || username.Length > maxLength)
        {
            return false;
        }

        return char.IsLetter(c: username[index: 0]) && username.All(predicate: char.IsLetterOrDigit);
    }

    public static bool IsAlphaString(this string target, int minLength = 0, int maxLength = int.MaxValue)
    {
        if (target.Length < minLength || target.Length > maxLength)
        {
            return false;
        }

        return target.All(predicate: char.IsLetter);
    }

    public static bool IsAlphaNumericString(this string target, int minLength = 0, int maxLength = int.MaxValue)
    {
        if (target.Length < minLength || target.Length > maxLength)
        {
            return false;
        }

        return target.All(predicate: char.IsLetterOrDigit);
    }

    public static bool IsAlphaNumericAndSpecialCharactersString(this string target, int minLength = 0,
        int maxLength = int.MaxValue)
    {
        if (target.Length < minLength || target.Length > maxLength)
        {
            return false;
        }

        const string specialCharacters = "!@#$%^&*()_+[]{}|;:,.<>?";

        return target.All(predicate: ch => char.IsLetterOrDigit(c: ch) || specialCharacters.Contains(value: ch));
    }

    public static bool IsValidUrl(this string url)
    {
        return Uri.TryCreate(uriString: url, uriKind: UriKind.Absolute, result: out Uri? uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}