using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Realms;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoMaui.Models;

namespace ToDoMaui.ViewsModes
{
    public partial class DashbordVM: BaseViewModel
    {
        private Realm realm;
        private PartitionSyncConfiguration config;

        public DashbordVM()
        {
            todoList  = new ObservableCollection<Todo>();
            EmptyText = "Niente da fare qui. Aggiungi nuove cose da fare per iniziare";

        }

        // constructor TodoList
        [ObservableProperty]
        ObservableCollection<Todo> todoList;

        [ObservableProperty]
        string emptyText;

        [ObservableProperty]     
        string todoEmptyText;

        [ObservableProperty]
        private bool isRefreshing;

        public async Task InitialiseRealm()
        {
            config = new PartitionSyncConfiguration($"{App.RealmApp.CurrentUser.Id}", App.RealmApp.CurrentUser);
            realm = Realm.GetInstance(config);
            
            GetTodos();

            if (TodoList.Count == 0)
            {
                EmptyText = "Carica progetto in corso...";
                await Task.Delay(2000);

                GetTodos();
                EmptyText = "Non Esiste un progetto. Aggiungi prima di Continuare!";
            }

        }

        [RelayCommand]
        public async void GetTodos()
        {
            IsRefreshing = true;
            IsBusy = true;

            try
            {
                var tlist = realm.All<Todo>().ToList().Reverse<Todo>().OrderBy(t => t.Completed);
                TodoList = new ObservableCollection<Todo> ( tlist ); 
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayPromptAsync("Errore!", ex.Message);
            }

            IsRefreshing = false;
            IsBusy = false;
        }

        [RelayCommand]
        public async void EditTodo(Todo td)
        {
            string newString = await App.Current.MainPage.DisplayPromptAsync("Edit", td.Name);

            if (newString is null || string.IsNullOrWhiteSpace(newString.ToString()))
                return;


            try
            {
                var foundTodo = realm.Find<Todo>(td.Id);

                foundTodo.Name = GeneralHelpers.UppercaseFirst(newString.ToString());
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayPromptAsync("Errore", ex.Message);
            }
        }

        [RelayCommand]
        public async void CheckTodo(Todo todo)
        {
            try
            {
                realm.Write(() =>
                {
                    var foundTodo = realm.Find<Todo>(todo.Id);

                    foundTodo.Completed = !foundTodo.Completed;
                });

                await Task.Delay(2);
               
                var sortedlist = TodoList.ToList().OrderBy(t => t.Completed);
                TodoList = new ObservableCollection<Todo>(sortedlist);
            }
            catch(Exception ex)
            {
                await App.Current.MainPage.DisplayPromptAsync("Errore", ex.Message);
            }
        }

        [RelayCommand]
        async Task SingOut()
        {
            IsBusy = true;

            try
            {
                //await App.RealmApp.RemoveUserAsync(App.RealmApp.CurrentUser);
                //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                //App.Current.Quit();

                var currentuserId = App.RealmApp.CurrentUser.Id;

                await App.RealmApp.RemoveUserAsync(App.RealmApp.CurrentUser);

                var noMoreCerrentUser = App.RealmApp.AllUsers.FirstOrDefault(u => u.Id == currentuserId);

                await Shell.Current.GoToAsync("///Login");
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayPromptAsync("Errore!", ex.Message);
            }
            IsBusy= false;
        }

        [RelayCommand]
        async Task AddTodo()
        {
            if (string.IsNullOrWhiteSpace(TodoEmptyText))
                return;

            IsBusy= true;

            try
            {
                var todo =
                    new Todo
                    {
                        Name = GeneralHelpers.UppercaseFirst(TodoEmptyText),
                        Partition = App.RealmApp.CurrentUser.Id,
                        Owner = App.RealmApp.CurrentUser.Profile.Email
                    };
                realm.Write(() =>
                {
                    realm.Add(todo);
                    todoEmptyText = "";
                });

                todoEmptyText = "";
                GetTodos();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayPromptAsync("Errore!", ex.Message);
            }
            

            IsBusy= false;
        }

        async Task DeleteTask(Todo todo)
        {
            IsBusy= true;

            try
            {
                realm.Write(() =>
                {
                    realm.Remove(todo);
                });

                todoList.Remove(todo);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayPromptAsync("Errore!", ex.Message);
            }

            IsBusy= false;
        }

    }
}
