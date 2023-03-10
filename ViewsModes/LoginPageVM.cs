using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoMaui.ViewsModes
{
    public partial class LoginPageVM: BaseViewModel
    {

        public LoginPageVM()
        {
            EmailText = "test@test.com";
            PasswordText = "testtest";
        }
        
        [ObservableProperty]
        string emailText;

        [ObservableProperty]
        string passwordText;

        public static async void StartDashbord()
        {
            await Shell.Current.GoToAsync("///Main");
        }

        [RelayCommand]
        private async void CreateAccoun()
        {
            try
            {
                await App.RealmApp.EmailPasswordAuth.RegisterUserAsync(EmailText, PasswordText);
                await Login();

            }catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Errore della creation dell'Accoun!", "Error: " + ex.Message, "Ok");
            }
        }


        [RelayCommand]
        public async Task Login()
        {
            try
            {
                var user = await App.RealmApp.LogInAsync(Credentials.EmailPassword(EmailText, PasswordText));

                if (user != null)
                {
                    await Shell.Current.GoToAsync("///Main");
                    EmailText = "";
                    PasswordText = "";
                }
                else
                {
                    throw new Exception();
                }
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Errore del login In ", ex.Message, "Ok");
            }
        }
    }

}
