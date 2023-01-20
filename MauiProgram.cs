using Microsoft.Extensions.DependencyInjection;
using ToDoMaui.Views;
using ToDoMaui.ViewsModes;

namespace ToDoMaui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<Dashbord>();
		builder.Services.AddSingleton<DashbordVM>();

		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<LoginPageVM>();

		return builder.Build();
	}
}
