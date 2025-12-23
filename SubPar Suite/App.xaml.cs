using ControlzEx.Theming;
using System.Windows;

namespace SubPar_Suite;

public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
		ThemeManager.Current.SyncTheme();
	}
}
