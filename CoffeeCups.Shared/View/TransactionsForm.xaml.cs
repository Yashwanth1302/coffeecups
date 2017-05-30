using CoffeeCups.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoffeeCups.View
{
	public partial class TransactionsForm : ContentPage
	{
        MasterViewModel vm;

        public TransactionsForm ()
		{
			InitializeComponent ();
            BindingContext = vm = new MasterViewModel(traid.Text, prodcod.Text, qty.Text);
             vm.ExecuteLoadCoffeesCommandAsync_Transaction();

        }

        public async void samplesave(Object o,EventArgs e)
        {

            BindingContext = vm = new MasterViewModel(traid.Text, prodcod.Text, qty.Text);
            await vm.ExecuteAddCoffeeCommandAsync();
            traid.Text = string.Empty;
            prodcod.Text = string.Empty;
            qty.Text = string.Empty;


        }

    }
}
