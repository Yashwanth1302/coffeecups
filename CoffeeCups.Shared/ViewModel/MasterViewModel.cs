using System;
using MvvmHelpers;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using CoffeeCups.Helpers;
using CoffeeCups.Model;

namespace CoffeeCups.ViewModel
{
   public class MasterViewModel : BaseViewModel
    {
        AzureService azureService;
        string tranid, productcode,qty;
        public MasterViewModel(string tranid, string productcode ,string qty)
        {
            this.tranid = tranid;
            this.productcode = productcode;
            this.qty = qty;
            azureService = DependencyService.Get<AzureService>();
        }

        public ObservableRangeCollection<TestingData> Coffees { get; } = new ObservableRangeCollection<TestingData>();
        public ObservableRangeCollection<TransactionData> transacts { get; } = new ObservableRangeCollection<TransactionData>();
        public ObservableRangeCollection<Grouping<string, TestingData>> CoffeesGrouped { get; set; } = new ObservableRangeCollection<Grouping<string, TestingData>>();
        public ObservableRangeCollection<Grouping<string, TransactionData>> CoffeesGrouped1 { get; set; } = new ObservableRangeCollection<Grouping<string, TransactionData>>();

        string loadingMessage;
        public string LoadingMessage
        {
            get { return loadingMessage; }
            set { SetProperty(ref loadingMessage, value); }
        }

        ICommand loadCoffeesCommand;
        public ICommand LoadCoffeesCommand =>
            loadCoffeesCommand ?? (loadCoffeesCommand = new Command(async () => await ExecuteLoadCoffeesCommandAsync()));

      public async Task ExecuteLoadCoffeesCommandAsync()
        {
            if (IsBusy || !(await LoginAsync()))
                return;


            try
            {
                LoadingMessage = "Loading Masters...";

                IsBusy = true;
                var coffees = await azureService.GetCoffees();
                Coffees.ReplaceRange(coffees);


                SortCoffees();


            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync coffees, you may be offline", "OK");
            }
            finally
            {
                IsBusy = false;
            }


        }
      public async Task ExecuteLoadCoffeesCommandAsync_Transaction()
        {
            if (IsBusy || !(await LoginAsync()))
                return;


            try
            {
                LoadingMessage = "Loading Masters...";
                IsBusy = true;
                var coffees = await azureService.GetCoffees_Transaction();
                transacts.ReplaceRange(coffees);


                SortCoffees_Transaction();


            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);

                await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync coffees, you may be offline", "OK");
            }
            finally
            {
                IsBusy = false;
            }


        }
        void SortCoffees()
        {
            var groups = from coffee in Coffees
                         orderby coffee.SkuCode descending
                         group coffee by coffee.SkuCode
                into coffeeGroup
                         select new Grouping<string, TestingData>($"{coffeeGroup.Key} ({coffeeGroup.Count()})", coffeeGroup);


            CoffeesGrouped.ReplaceRange(groups);
        }


        void SortCoffees_Transaction()
        {
            var groups = from coffee in transacts
                         orderby coffee.Productcode descending
                         group coffee by coffee.Productcode
                into coffeeGroup
                         select new Grouping<string, TransactionData>($"{coffeeGroup.Key} ({coffeeGroup.Count()})", coffeeGroup);


            CoffeesGrouped1.ReplaceRange(groups);
        }

        /*
                bool atHome;
                public bool AtHome
                {
                    get { return atHome; }
                    set { SetProperty(ref atHome, value); }
                }
                */

        //Add data to local as well as uploading data to azure

        //ICommand addCoffeeCommand;
        //public ICommand AddCoffeeCommand =>
        //    addCoffeeCommand ?? (addCoffeeCommand = new Command(async () => await ExecuteAddCoffeeCommandAsync()));


        public async Task ExecuteAddCoffeeCommandAsync()
        {
            if (IsBusy || !(await LoginAsync()))
                return;

            try
            {
                LoadingMessage = "Adding Transaction...";
                IsBusy = true;


                var coffee = await azureService.AddCoffee(tranid, productcode,qty);
                transacts.Add(coffee);
            //    SortCoffees();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OH NO!" + ex);
            }
            finally
            {
                LoadingMessage = string.Empty;
                IsBusy = false;
            }

        }

        public Task<bool> LoginAsync()
        {
            if (Settings.IsLoggedIn)
                return Task.FromResult(true);


            return azureService.LoginAsync();
        }
    }
}
