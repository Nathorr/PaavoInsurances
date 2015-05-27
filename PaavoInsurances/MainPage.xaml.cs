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
        private void MainScanMeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CameraPage), "homePage");
        }
        
        private async void CreateDatabase()
        {
            SQLiteAsyncConnection conn2 = new SQLiteAsyncConnection("ClientTable");
            await conn2.CreateTableAsync<ClientTable>();
        }
        private async void FillDataBase()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");

            ClientTable table = new ClientTable
            {
                Id = "5564a5b90cf2ffffdde2e62b",
                securityId = "191093-1472",
                bonusCardNumber = "123123123123123",
            };

            await conn.InsertAsync(table);
        }
        private async void FetchData()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");

            var query = conn.Table<ClientTable>().Where(x => x.Id == "5564a5b90cf2ffffdde2e62b");
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                Debug.WriteLine(string.Format("{0}: {1} ", item.Id, item.securityId));
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //CreateDatabase();
            //FillDataBase();
            //FetchData();
        }
    }
}
