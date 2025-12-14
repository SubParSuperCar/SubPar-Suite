using ControlzEx.Theming;
using System.Windows;
using System.Windows.Threading;

namespace SubPar_Suite;

public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
		{
			if (e.ExceptionObject is Exception ex)
				Shared.ShowError(ex, "Unhandled Background Exception",
					"An unhandled exception was thrown in a background thread. The app will shut down after this dialog is closed.");
		};

		ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
		ThemeManager.Current.SyncTheme();
	}

	private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		Shared.ShowError(e.Exception, "Unhandled Exception",
			"An unhandled exception was thrown in the UI thread. The app will shut down after this dialog is closed.");

		e.Handled = true;
	}
}
