﻿using CoffeeCups.ViewModel;
using Xamarin.Forms;
using Plugin.Connectivity;
using CoffeeCups.Helpers;

namespace CoffeeCups.View
{
	public partial class Masterpage : ContentPage
    {
        string tranid, productcode,qty;
        MasterViewModel vm;
        public Masterpage ()
		{
			InitializeComponent ();
           BindingContext = vm = new MasterViewModel(tranid,productcode,qty);
            ListViewCoffees.ItemTapped += (sender, e) =>
            {
                if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
                    ListViewCoffees.SelectedItem = null;
            };

            if (Device.OS != TargetPlatform.iOS && Device.OS != TargetPlatform.Android)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Command = vm.LoadCoffeesCommand
                });
            }


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            CrossConnectivity.Current.ConnectivityChanged += ConnecitvityChanged;
            OfflineStack.IsVisible = !CrossConnectivity.Current.IsConnected;


            if (vm.Coffees.Count == 0 && Settings.IsLoggedIn)
                vm.LoadCoffeesCommand.Execute(null);
            else
            {
                await vm.LoginAsync();
                if (Settings.IsLoggedIn)
                    vm.LoadCoffeesCommand.Execute(null);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CrossConnectivity.Current.ConnectivityChanged -= ConnecitvityChanged;
        }

        void ConnecitvityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                OfflineStack.IsVisible = !e.IsConnected;
            });
        }
        private void listitemclick(object sender, SelectedItemChangedEventArgs e)
        {
            // var item = e.SelectedItem;
            Navigation.PushModalAsync(new TransactionsForm());
        }

    }
}
