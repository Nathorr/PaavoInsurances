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
            //result = e.Parameter.ToString();
            cameraClass = (CameraClass)e.Parameter;

            GetUserFromSqlite(cameraClass);

        }
        async private void GetUserFromSqlite(CameraClass cameraClass)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");
            var query = conn.Table<ClientTable>().Where(x => x.securityId == cameraClass.homeInsuranceClass.id);
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                getJson(item.Id);
            }

        }
        
        public class CameraClass
        {
            public HomeInsuranceClass homeInsuranceClass = new HomeInsuranceClass();
            public string previousPage { get; set; }

        }

        public class HomeInsuranceClass
        {
            public PricingParameters pricingParameters = new PricingParameters();
            public string id { get; set; }
            public string name { get; set; }
            public string surName { get; set; }
            public string validTo { get; set; }
            public string bonusCard { get; set; }

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
            Debug.WriteLine(response);
            using (HttpContent content = response.Content)
            {
                var result = await content.ReadAsStringAsync();
                Debug.WriteLine(result);
                List<HomeInsuranceClass> deSer = JsonConvert.DeserializeObject<List<HomeInsuranceClass>>(result);
                for (int i = 0; i < deSer.Count; i++)
                {
                    Debug.WriteLine("Eri id:t : " +deSer[i].id);
                    Debug.WriteLine("Täs lorpon hetulla vakuutusID: " + insuranceId);
                    if(deSer[i].id == insuranceId)
                    {
                        Debug.WriteLine("Jee päästiin tänne asti!");
                        OldCustomerFirstNameTextBox.Text = deSer[i].name;
                        OldCustomerLastNameTextBox.Text = deSer[i].surName;
                        OldCustomerAddressTextBox.Text = deSer[i].pricingParameters.address;
                        OldCustomerPostalCodeTextBox.Text = deSer[i].pricingParameters.postalCode;
                        OldCustomerHouseTypeTextBox.Text = deSer[i].pricingParameters.houseType;
                        OldCustomerBuildYearTextBox.Text = deSer[i].pricingParameters.buildYear;
                        OldCustomerAreaTextBox.Text = deSer[i].pricingParameters.area;
                        OldCustomerPriceBox.Text = deSer[i].pricingParameters.price.price + " " + deSer[i].pricingParameters.currency;
                    }
                    else
                    {
                        Debug.WriteLine("Nothing found!!!!");
                    }
                    
                }
            }
        }

        private void OldCustomerScanMeButton_Click(object sender, RoutedEventArgs e)
        {
            CameraClass cameraClass = new CameraClass
            {
                previousPage = "ScannedOldCustomerInfoPage",
                homeInsuranceClass = new HomeInsuranceClass
                {
                    id = result
                }

            };

            this.Frame.Navigate(typeof(CameraPage), cameraClass);

        }

        
    }
}
