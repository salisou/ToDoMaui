using ToDoMaui.ViewsModes;

namespace ToDoMaui.Views;

public partial class Dashbord : ContentPage
{

	DashbordVM vm; 


    public Dashbord()
	{
		InitializeComponent();
		vm = new DashbordVM();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
		await vm.InitialiseRealm();
    }
}