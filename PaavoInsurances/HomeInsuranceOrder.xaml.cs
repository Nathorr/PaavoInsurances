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
using System.Collections;

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
            OrderIDTextBox.Text = cameraClass.socialSecurityId;
            GetUserFromSqlite(cameraClass);
            if(cameraClass.bonusCard != null)
                OrderBonusCardTextBox.Text = cameraClass.bonusCard;
            
            OrderPriceTextBox.Text = cameraClass.homeInsuranceClass.pricingParameters.price.price;
            
        }

        public class HomeInsuranceClass
        {
            public PricingParameters pricingParameters = new PricingParameters();
            public string name { get; set; }
            public string surName { get; set; }
            public string validTo { get; set; }
            public string id { get; set; }
            
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
        //public HomeInsuranceClass homeInsurance = new HomeInsuranceClass();
       
        async private void InsertId(string id, string scanId)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");

            ClientTable client = new ClientTable
            {
                Id = id,
                securityId = scanId,
            };

            await conn.InsertAsync(client);
        }

        async private void insertBonusCard(string bonusCard, string id, string scanId, ScannedOldCustomerInfo.CameraClass cameraClass)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");
            var query = conn.Table<ClientTable>().Where(x => x.securityId == cameraClass.socialSecurityId);
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                Debug.WriteLine("Item.id : ´"+id);
                cameraClass.homeInsuranceClass.id = id;
                Debug.WriteLine("cameraClass id: "+id);
                Debug.WriteLine("SAMA VITUN HETU!");
            }
            if(result.Count < 1)    
            {
                Debug.WriteLine("Testiprinttii: "+id + " " + bonusCard + " " + scanId);
                ClientTable client = new ClientTable
                {
                    Id = id,
                    bonusCardNumber = bonusCard,
                    securityId = scanId,
                };
                await conn.InsertAsync(client);
            }
                
            
           
            

        }

        /*private async void GetDataFromSqlite()
        {
            Debug.WriteLine(cameraClass.socialSecurityId);
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");
            
        }*/

        async private void GetUserFromSqlite(ScannedOldCustomerInfo.CameraClass cameraClass)
        {
            Debug.WriteLine(cameraClass.socialSecurityId);
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");
            var query = conn.Table<ClientTable>().Where(x => x.securityId == cameraClass.socialSecurityId);
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                /*if(item.securityId == cameraClass.socialSecurityId)
                {

                }*/
                if (item.bonusCardNumber != null)
                {
                    OrderBonusCardTextBox.Text = item.bonusCardNumber;
                    ScanMeButton.Visibility = Visibility.Collapsed;

                }
                getJson(item.Id);
            }
        }
        public async void getJson(string insuranceId)
        {
            
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
                    Debug.WriteLine("Eri id:t : " + deSer[i].id);
                    Debug.WriteLine("Täs lorpon hetulla vakuutusID: " + insuranceId);
                    if (deSer[i].id == insuranceId)
                    {
                        OrderFirstNameTextBox.Text = deSer[i].name;
                        OrderLastNameTextBox.Text = deSer[i].surName;
                        
                    }
                    else
                    {
                        Debug.WriteLine("Nothing found!!!!");
                    }

                }
            }
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
            homeInsurance.pricingParameters.price.billingPeriod = "YEAR";
            
            //string sr_string = JsonConvert.SerializeObject(homeInsurance, Formatting.Indented);
            //Debug.WriteLine("Post: " +sr_string);

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
                Debug.WriteLine(result);
                InsertId(result, "asdasda");
            }*/

        }

        private void ScanMeButton_Click(object sender, RoutedEventArgs e)
        {
            cameraClass.previousPage = "homeInsuranceOrder";
            this.Frame.Navigate(typeof(CameraPage), cameraClass);
        }

        private void ArrowBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomeInsuranceOffer));
        }

        private async void ConfirmationYesButton_Click(object sender, RoutedEventArgs e)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("ClientTable");
            var query = conn.Table<ClientTable>().Where(x => x.securityId == cameraClass.socialSecurityId);
            var queryResult = await query.ToListAsync();
            foreach (var item in queryResult)
            {
                Debug.WriteLine("Item.id : ´" + item.Id);
                cameraClass.homeInsuranceClass.id = item.Id;
                Debug.WriteLine("cameraClass id: " + item.Id);
                Debug.WriteLine("SAMA VITUN HETU!");
            }
            Debug.WriteLine("ID funktiossa: "+cameraClass.homeInsuranceClass.id);
            cameraClass.homeInsuranceClass.name = OrderFirstNameTextBox.Text;
            cameraClass.homeInsuranceClass.surName = OrderLastNameTextBox.Text;
            cameraClass.homeInsuranceClass.validTo = OrderValidToDatePicker.Date.Day.ToString() + "." + OrderValidToDatePicker.Date.Month.ToString() + "." + OrderValidToDatePicker.Date.Year.ToString();

            string sr_string = JsonConvert.SerializeObject(cameraClass.homeInsuranceClass, Formatting.Indented);
            Debug.WriteLine("Post: " + sr_string);

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
                Debug.WriteLine("Id tässä vaiheessa1: " + cameraClass.homeInsuranceClass.id);
                //ID joka yhistetään tauluun henkkaritunnuksen kanssa.
                var result = await content.ReadAsStringAsync();
                Debug.WriteLine("Result palvelimelta: " + result);
                
                ReturnId deSer = JsonConvert.DeserializeObject<ReturnId>(result);
                //InsertId(deSer.id, cameraClass.socialSecurityId);
                Debug.WriteLine("Id tässä vaiheessa2: " + cameraClass.homeInsuranceClass.id);
                Debug.WriteLine("Deser id: " + deSer.id);
                insertBonusCard(cameraClass.bonusCard, deSer.id, cameraClass.socialSecurityId, cameraClass);
            }
            if (ConfirmPopup.IsOpen == true)
            {
                ConfirmPopup.IsOpen = false;
                Frame.Navigate(typeof(ThankYouPage));
            }
        }

        private void ConfirmationNoButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmPopup.IsOpen == true)
                ConfirmPopup.IsOpen = false;
        }

        private void ArrowForwardButton_Click(object sender, RoutedEventArgs e)
        {
            // Tähän voi valmistella tallennusta tietokantoihin, esim. luokkiin sitomiset
            ConfirmPopup.IsOpen = true;
        }
    }
}
