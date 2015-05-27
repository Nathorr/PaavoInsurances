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
            //this.Frame.Navigate(typeof(CameraPage), new ScannedOldCustomerInfo.CameraClass { previousPage = "homePage" });
        }

        private void MainOfferMeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomeInsuranceOffer));
        }

        
        private async void CreateDatabase()
        {
            Debug.WriteLine("Funktiossa");
            SQLiteAsyncConnection conn2 = new SQLiteAsyncConnection("SpamTable");
            await conn2.CreateTableAsync<SpamTable>();
            Debug.WriteLine("Taulu pitäisi olla luotu!");
        }
        private async void FillDataBase()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("SpamTable");

            SpamTable table = new SpamTable
            {
                name = "1",
            };
            await conn.InsertAsync(table);
        }
        private async void FetchData()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("SpamTable");

            var query = conn.Table<SpamTable>();
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                Debug.WriteLine(string.Format("{0}", item.name));
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
