using ToDoMaui.ViewsModes;

namespace ToDoMaui.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageVM vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}