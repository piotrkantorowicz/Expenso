using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text.RegularExpressions;

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
        if (string.IsNullOrWhiteSpace(value: host) || host.Length > 255 || host.Contains(value: ' '))
        {
            return false;
        }

        UriHostNameType hostType = Uri.CheckHostName(name: host);

        return hostType switch
        {
            UriHostNameType.Dns => IsValidDnsHost(host: host),
            UriHostNameType.IPv4 => IsValidIPv4Host(host: host),
            UriHostNameType.IPv6 => IsValidIPv6Host(host: host),
            _ => false
        };
    }

    public static bool IsValidPassword(this string? password, int minLength = 8, int maxLength = 128)
    {
        if (string.IsNullOrWhiteSpace(value: password) || password.Length < minLength || password.Length > maxLength)
        {
            return false;
        }

        bool hasUppercase = password.Any(predicate: char.IsUpper);
        bool hasLowercase = password.Any(predicate: char.IsLower);
        bool hasDigit = password.Any(predicate: char.IsDigit);
        bool hasSpecialChar = password.Any(predicate: "!@#$%^&*()_+[]{}|;:,.<>?".Contains);

        return hasUppercase && hasLowercase && hasDigit && hasSpecialChar && !password.Contains(value: ' ');
    }

    public static bool IsValidUsername(this string? username, int minLength = 3, int maxLength = 30)
    {
        if (string.IsNullOrWhiteSpace(value: username) || username.Length < minLength || username.Length > maxLength)
        {
            return false;
        }

        return char.IsLetter(c: username[index: 0]) &&
               username.All(predicate: c => char.IsLetterOrDigit(c: c) && !char.IsWhiteSpace(c: c));
    }

    public static bool IsAlphaString(this string? target, int minLength = 0, int maxLength = int.MaxValue)
    {
        if (string.IsNullOrWhiteSpace(value: target) || target.Length < minLength || target.Length > maxLength)
        {
            return false;
        }

        return target.All(predicate: ch => char.IsLetter(c: ch) && !char.IsWhiteSpace(c: ch));
    }

    public static bool IsAlphaNumericString(this string? target, int minLength = 0, int maxLength = int.MaxValue)
    {
        if (string.IsNullOrWhiteSpace(value: target) || target.Length < minLength || target.Length > maxLength)
        {
            return false;
        }

        return target.All(predicate: ch => char.IsLetterOrDigit(c: ch) && !char.IsWhiteSpace(c: ch));
    }

    public static bool IsAlphaNumericAndSpecialCharactersString(this string? target, int minLength = 0,
        int maxLength = int.MaxValue, string specialCharacters = "!@#$%^&*()_+[]{}|;:,.<>?")
    {
        if (string.IsNullOrWhiteSpace(value: target) || target.Length < minLength || target.Length > maxLength)
        {
            return false;
        }

        return target.All(predicate: ch =>
            (char.IsLetterOrDigit(c: ch) || specialCharacters.Contains(value: ch)) && !char.IsWhiteSpace(c: ch));
    }

    public static bool IsValidUrl(this string url)
    {
        return Uri.TryCreate(uriString: url, uriKind: UriKind.Absolute, result: out Uri? uriResult) &&
               (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    public static bool IsValidRelativePath(this string? path)
    {
        if (string.IsNullOrWhiteSpace(value: path))
        {
            return false;
        }

        char[] additionalInvalidChars = [':', '*', '?', '"', '<', '>', '|'];

        if (path.Any(predicate: pathChar =>
                char.IsControl(c: pathChar) || GetInvalidPathChars(additionalInvalidChars: additionalInvalidChars)
                    .Contains(value: pathChar)))
        {
            return false;
        }

        return !Path.IsPathRooted(path: path);
    }

    public static bool IsValidRootPath(this string? path)
    {
        if (string.IsNullOrWhiteSpace(value: path))
        {
            return false;
        }

        char[] additionalInvalidChars = ['*', '?', '"', '<', '>', '|'];

        if (path.Any(predicate: pathChar =>
                GetInvalidPathChars(additionalInvalidChars: additionalInvalidChars).Contains(value: pathChar)))
        {
            return false;
        }

        try
        {
            string fullPath = Path.GetFullPath(path: path);

            return Path.IsPathRooted(path: fullPath) && Path.GetPathRoot(path: fullPath) == fullPath;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidDnsHost(string host)
    {
        const string dnsPattern = "^(?!-)[A-Za-z0-9-]{1,63}(?<!-)$";

        return host.Split(separator: '.').All(predicate: label => Regex.IsMatch(input: label, pattern: dnsPattern));
    }

    private static bool IsValidIPv4Host(string host)
    {
        return IPAddress.TryParse(ipString: host, address: out IPAddress? ip) &&
               ip.AddressFamily == AddressFamily.InterNetwork;
    }

    private static bool IsValidIPv6Host(string host)
    {
        return IPAddress.TryParse(ipString: host, address: out IPAddress? ip) &&
               ip.AddressFamily == AddressFamily.InterNetworkV6;
    }

    private static char[] GetInvalidPathChars(char[] additionalInvalidChars)
    {
        char[] generalInvalidChars = Path.GetInvalidPathChars();

        return generalInvalidChars.Concat(second: additionalInvalidChars).ToArray();
    }
}