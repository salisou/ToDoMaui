using ToDoMaui.Views;

namespace ToDoMaui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(Dashbord), typeof(Dashbord));
	}
}
