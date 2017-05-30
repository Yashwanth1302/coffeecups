//#define AUTH

using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Diagnostics;
using Xamarin.Forms;
using CoffeeCups.Helpers;
using CoffeeCups.Authentication;
using CoffeeCups;
using System.IO;
using Plugin.Connectivity;
using CoffeeCups.Model;

[assembly: Dependency(typeof(AzureService))]
namespace CoffeeCups
{
    public class AzureService
    {

        // public MobileServiceClient Client { get; set; } = null;

        public MobileServiceClient Client = null;
      public static  IMobileServiceSyncTable<TestingData> coffeeTable;

        public static IMobileServiceSyncTable<TransactionData> transactiontable;

        public static bool UseAuth { get; set; } = false;

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;
            var appUrl = "http://sampletestingdata.azurewebsites.net";
           
#if AUTH      
            Client = new MobileServiceClient(appUrl, new AuthHandler());

            if (!string.IsNullOrWhiteSpace (Settings.AuthToken) && !string.IsNullOrWhiteSpace (Settings.UserId)) {
                Client.CurrentUser = new MobileServiceUser (Settings.UserId);u
                Client.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
            }
#else
            //Create our client

            Client = new MobileServiceClient(appUrl);

#endif

            //InitialzeDatabase for path
            var path = "masterdata.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);

            //Define table
            store.DefineTable<TestingData>();
           // store.DefineTable<TransactionData>();

            //Initialize SyncContext
            await Client.SyncContext.InitializeAsync(store);

            //Get our sync table that will call out to azure
            coffeeTable = Client.GetSyncTable<TestingData>();
            //transactiontable = Client.GetSyncTable<TransactionData>();

            
        } 

        public async Task SyncCoffee()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await coffeeTable.PullAsync("sampletestingdata", coffeeTable.CreateQuery());

                await Client.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync coffees, that is alright as we have offline capabilities: " + ex);
            }

        }

        public async Task SyncCoffee_Transaction()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected)
                    return;

                await transactiontable.PullAsync("sampletestingdata", transactiontable.CreateQuery());

                await Client.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync coffees, that is alright as we have offline capabilities: " + ex);
            }

        }
        public async Task<IEnumerable<TestingData>> GetCoffees()
        {
            //Initialize & Sync
            await Initialize();
            await SyncCoffee();

            // return await coffeeTable.OrderBy(c => c.DateUtc).ToEnumerableAsync(); ;

            var data = await coffeeTable.OrderBy(c => c.SkuCode).ToEnumerableAsync();

            return data;


        }


        public async Task<IEnumerable<TransactionData>> GetCoffees_Transaction()
        {
            //Initialize & Sync
            await Initialize();
            await SyncCoffee();

            // return await coffeeTable.OrderBy(c => c.DateUtc).ToEnumerableAsync(); ;

            var data = await transactiontable.OrderBy(c => c.Transactionid).ToEnumerableAsync();

            return data;


        }

        public async Task<TransactionData> AddCoffee(string tranid,string prodcode,string qty)
        {
            await Initialize();

            var coffee = new TransactionData
            {
                Transactionid = tranid,//"343536",
                Productcode = prodcode,
                Qty = qty,
                //    OS = Device.OS.ToString()
            };

            await transactiontable.InsertAsync(coffee);

            await SyncCoffee_Transaction();
            //return coffee
            return coffee;
        }



        public async Task<bool> LoginAsync()
        {

            await Initialize();

            var auth = DependencyService.Get<IAuthentication>();
            var user = await auth.LoginAsync(Client, MobileServiceAuthenticationProvider.Twitter);

            if (user == null)
            {
                Settings.AuthToken = string.Empty;
                Settings.UserId = string.Empty;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("Login Error", "Unable to login, please try again", "OK");
                });
                return false;
            }
            else
            {
                Settings.AuthToken = user.MobileServiceAuthenticationToken;
                Settings.UserId = user.UserId;
            }

            return true;
        }
    }
}

