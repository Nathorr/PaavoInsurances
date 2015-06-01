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
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using SQLite;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PaavoInsurances
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScannedOldCustomerInfo : Page
    {
        public string result;
        public CameraClass cameraClass;

        public ScannedOldCustomerInfo()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            cameraClass = (CameraClass)e.Parameter;
            if(cameraClass.previousPage == "scannedOldCustomerInfoPage")
            {
                OldCustomerBonusCard.Text = cameraClass.bonusCard;
            }
            GetUserFromSqlite(cameraClass);

        }
        async private void GetUserFromSqlite(CameraClass cameraClass)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");
            var query = conn.Table<ClientTable>().Where(x => x.securityId == cameraClass.socialSecurityId);
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                if (item.bonusCardNumber != null)
                {
                    OldCustomerBonusCard.Text = item.bonusCardNumber;
                    OldCustomerScanMeButton.Visibility = Visibility.Collapsed;
                }
                getJson(item.Id);
            }
        }

        async private void insertBonusCard(string bonusCard, string id, string scanId, ScannedOldCustomerInfo.CameraClass cameraClass)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");
            
            var query = conn.Table<ClientTable>().Where(x => x.securityId == cameraClass.socialSecurityId);
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                
                ClientTable client = new ClientTable
                {
                    Id = id,
                    securityId = scanId,
                    bonusCardNumber = bonusCard,
                    
                };
                await conn.UpdateAsync(client);
            }
        }

        private async void FetchData()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");

            var query = conn.Table<ClientTable>();
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
            }
        }
        
        public class CameraClass
        {
            public HomeInsuranceClass homeInsuranceClass = new HomeInsuranceClass();
            public string previousPage { get; set; }
            public string bonusCard { get; set; }
            public string socialSecurityId { get; set; }
        }

        public class HomeInsuranceClass
        {
            public PricingParameters pricingParameters = new PricingParameters();
            public string id { get; set; }
            public string name { get; set; }
            public string surName { get; set; }
            public string validTo { get; set; }
            

        }
        public class PricingParameters
        {
            public InsurancePrice price = new InsurancePrice();
            public string postalCode { get; set; }
            public string address { get; set; }
            public string area { get; set; }
            public string buildYear { get; set; }
            public string insuranceStartDate { get; set; }
            public string houseType { get; set; }
            public string currency { get; set; }
            public string billingPeriod { get; set; }

        }
        public class InsurancePrice
        {
            public string price { get; set; }
            public string currency { get; set; }
            public string billingPeriod { get; set; }
        }
        public class ReturnId
        {
            public string id { get; set; }
        }
        public HomeInsuranceClass homeInsurance = new HomeInsuranceClass();

        

        public async void getJson(string insuranceId)
        {
            OldCustomerFirstNameTextBox.Text = "";
            string RequestUrl = "http://185.20.136.51/sellertool/applications/";

            HttpClient clientOb = new HttpClient();
            string plain = "LUT" + ":" + "0gmsl48hgi_jhfiud76";
            string authString = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
            clientOb.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
            clientOb.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await clientOb.GetAsync(RequestUrl);
            using (HttpContent content = response.Content)
            {
                var result = await content.ReadAsStringAsync();
                List<HomeInsuranceClass> deSer = JsonConvert.DeserializeObject<List<HomeInsuranceClass>>(result);
                for (int i = 0; i < deSer.Count; i++)
                {
                    if(deSer[i].id == insuranceId)
                    {
                        OldCustomerFirstNameTextBox.Text = deSer[i].name;
                        OldCustomerLastNameTextBox.Text = deSer[i].surName;
                        OldCustomerAddressTextBox.Text = deSer[i].pricingParameters.address;
                        OldCustomerPostalCodeTextBox.Text = deSer[i].pricingParameters.postalCode;
                        OldCustomerHouseTypeTextBox.Text = deSer[i].pricingParameters.houseType;
                        OldCustomerBuildYearTextBox.Text = deSer[i].pricingParameters.buildYear;
                        OldCustomerAreaTextBox.Text = deSer[i].pricingParameters.area;
                        OldCustomerPriceBox.Text = deSer[i].pricingParameters.price.price + " " + deSer[i].pricingParameters.currency;
                        cameraClass.homeInsuranceClass.id = deSer[i].id;
                        
                    }
                    
                }
            }
        }

        private void OldCustomerScanMeButton_Click(object sender, RoutedEventArgs e)
        {
            cameraClass.previousPage = "scannedOldCustomerInfoPage";
            this.Frame.Navigate(typeof(CameraPage), cameraClass);

        }

        private void ArrowBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void OldCustomerOrderMeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomeInsuranceOffer));
        }

        private void ArrowForwardButton_Click(object sender, RoutedEventArgs e)
        {
            insertBonusCard(cameraClass.bonusCard, cameraClass.homeInsuranceClass.id , cameraClass.socialSecurityId, cameraClass);
            this.Frame.Navigate(typeof(BonusClubThankYouPage));
        }

        
    }
}
