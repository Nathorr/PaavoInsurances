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
    public sealed partial class HomeInsuranceOrder : Page
    {
        public ScannedOldCustomerInfo.CameraClass cameraClass;

        public HomeInsuranceOrder()
        {
            this.InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            cameraClass = (ScannedOldCustomerInfo.CameraClass)e.Parameter;
            //OrderFirstNameTextBox.Text = cameraClass.homeInsuranceClass.name;
            //OrderLastNameTextBox.Text = cameraClass.homeInsuranceClass.surName;
            OrderIDTextBox.Text = cameraClass.homeInsuranceClass.id;
            if(cameraClass.homeInsuranceClass.bonusCard != null)
                OrderBonusCardTextBox.Text = cameraClass.homeInsuranceClass.bonusCard;
            OrderPriceTextBox.Text = cameraClass.homeInsuranceClass.pricingParameters.price.price;
        }

        public class HomeInsuranceClass
        {
            public PricingParameters pricingParameters = new PricingParameters();
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
       
        async private void InsertId(string id, string scanId)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("clientTable");

            ClientTable client = new ClientTable
            {
                Id = id,
                securityId = scanId,
            };

            await conn.InsertAsync(client);
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*homeInsurance.name = "asdasd";
            homeInsurance.surName = "asdasd";
            homeInsurance.validTo = "13.07.2015";
            homeInsurance.pricingParameters.postalCode = "00100";
            homeInsurance.pricingParameters.address = "asdasdasd";
            homeInsurance.pricingParameters.area = "79.5";
            homeInsurance.pricingParameters.buildYear = "1520";
            homeInsurance.pricingParameters.insuranceStartDate = "12.08.2016";
            homeInsurance.pricingParameters.houseType = "APARTMENT";
            homeInsurance.pricingParameters.currency = "EUR";
            homeInsurance.pricingParameters.billingPeriod = "YEAR";
            homeInsurance.pricingParameters.price.price = "280";
            homeInsurance.pricingParameters.price.currency = "EUR";
            homeInsurance.pricingParameters.price.billingPeriod = "YEAR";*/
            
            string sr_string = JsonConvert.SerializeObject(this.homeInsurance, Formatting.Indented);
            Debug.WriteLine("Post: " +sr_string);

            string RequestUrl = "http://185.20.136.51/sellertool/applications/";

            HttpClient clientOb = new HttpClient();
            string plain = "LUT" + ":" + "0gmsl48hgi_jhfiud76";
            string authString = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
            clientOb.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
            clientOb.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await clientOb.PostAsync(RequestUrl, new StringContent(sr_string, Encoding.UTF8, "application/json"));
            Debug.WriteLine(response);
            using (HttpContent content = response.Content)
            {
                //ID joka yhistetään tauluun henkkaritunnuksen kanssa.
                var result = await content.ReadAsStringAsync();
                InsertId(result, "asdasda");
            }

        }

        private void ScanMeButton_Click(object sender, RoutedEventArgs e)
        {
            cameraClass.previousPage = "homeInsuranceOrder";
            this.Frame.Navigate(typeof(CameraPage), cameraClass);
        }


    }
}
