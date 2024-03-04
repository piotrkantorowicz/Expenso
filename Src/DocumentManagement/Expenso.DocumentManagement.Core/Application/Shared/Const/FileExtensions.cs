namespace Expenso.DocumentManagement.Core.Application.Shared.Const;

internal static class FileExtensions
{
    public const string Csv = ".csv";
    public const string Xlsx = ".xlsx";
    public static readonly string[] SupportedExtensions = [Csv, Xlsx];
}