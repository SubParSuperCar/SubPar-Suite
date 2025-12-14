using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace SubPar_Suite;

public static partial class Shared
{
	public static Assembly Assembly => Assembly.GetExecutingAssembly();
	public static string AssemblyName => Assembly.GetName().Name ?? "<Assembly Name Null>";

	private static string FormatException(Exception ex)
	{
		return $"Message: \"{ex.Message}\"\n" +
			   $"Type: \"{ex.GetType().FullName}\"\n\n" +
			   $"Source (Assembly): \"{ex.Source ?? "<Null>"}\"\n" +
			   $"Target Site (Method): \"{ex.TargetSite?.ToString() ?? "<Null>"}\"\n\n" +
			   $"HResult (Error Code): 0x{ex.HResult:X8}\n\n" +
			   $"Stack Trace (Execution Path):\n\"{ex.StackTrace ?? "<Null>"}\"";
	}

	public static void ShowError(Exception ex, string title = "Error", string? header = null)
	{
		var message = FormatException(ex);

		if (ex.InnerException != null)
			message = $"--- Outer Exception ---\n\n{message}\n\n" +
					  $"--- Inner Exception ---\n\n{FormatException(ex.InnerException)}";

		if (!string.IsNullOrWhiteSpace(header))
			message = $"{header}\n\n{message}";

		MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
	}

	[GeneratedRegex(@"(\B[A-Z])", RegexOptions.Compiled)]
	private static partial Regex CamelCaseRegex();

	public static string SplitCamelCase(string input)
	{
		return CamelCaseRegex().Replace(input, " $1");
	}

	private const string UrlNormalizationEscape = "\\";
	private const string UrlSchemeDefault = "https";

	private static readonly string[] RecognizedUrlSchemes = {
		"http", "https", "file"
	};

	public static string NormalizeUrl(string url)
	{
		if (string.IsNullOrWhiteSpace(url))
			return string.Empty;

		url = url.Trim();

		if (url.StartsWith(UrlNormalizationEscape))
			return url.Substring(1);

		if (RecognizedUrlSchemes.Any(scheme => url.StartsWith(scheme + "://")))
			return url;

		return $"{UrlSchemeDefault}://{url}";
	}
}
