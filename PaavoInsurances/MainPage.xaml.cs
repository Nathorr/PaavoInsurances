using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PaavoInsurances
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ContactInfoPage));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomeInsuranceOrder));
        }
        private async void CreateDatabase()
        {
            SQLiteAsyncConnection conn1 = new SQLiteAsyncConnection("spamTable");
            await conn1.CreateTableAsync<SpamTable>();
            SQLiteAsyncConnection conn2 = new SQLiteAsyncConnection("clientTable");
            await conn2.CreateTableAsync<ClientTable>();
        }
        private async void FillDataBase()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("spamTable");

            SpamTable spam = new SpamTable
            {
                name = "Matteo Perse",
                home = true,

            };

            await conn.InsertAsync(spam);
        }
        private async void FetchData()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("spamTable");

            var query = conn.Table<SpamTable>().Where(x => x.name == "Matteo Perse");
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                Debug.WriteLine(string.Format("{0}: {1} {2}", item.Id, item.name, item.home));
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FillDataBase();
            FetchData();
        }
    }
}
