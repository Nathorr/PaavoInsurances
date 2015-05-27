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
    public sealed partial class HomeInsuranceOffer : Page
    {
        public HomeInsuranceOffer()
        {
            this.InitializeComponent();
        }
        public class ReturnedData
        {
            [DataMember(Name = "postalCode")]
            public string postalCode { get; set; }
            [DataMember(Name = "address")]
            public string address { get; set; }
            [DataMember(Name = "area")]
            public float area { get; set; }
            [DataMember(Name = "buildYear")]
            public int buildYear { get; set; }
            [DataMember(Name = "insuranceStartDate")]
            public string insuranceStartDate { get; set; }
            [DataMember(Name = "houseType")]
            public string houseType { get; set; }
            [DataMember(Name = "billingPeriod")]
            public string billingPeriod { get; set; }
            [DataMember(Name = "currency")]
            public string currency { get; set; }
            [DataMember(Name = "price")]
            public InsurancePrice price;

        }
        [DataContract]
        public class InsurancePrice
        {
            [DataMember(Name = "price")]
            public double price;
            [DataMember(Name = "currency")]
            public string currency;
            [DataMember(Name = "billingPeriod")]
            public string billingPeriod;
        }


        private void ArrowForwardButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private async void OfferPriceMeButton_Click(object sender, RoutedEventArgs e)
        {

            Price price = new Price();

            if (HouseToggleButton.IsChecked == true)
            {
                ApartmentToggleButton.IsChecked = false;
                RowHouseToggleButton.IsChecked = false;
                PairHouseToggleButton.IsChecked = false;
                SummerHouseToggleButton.IsChecked = false;
                SaunaToggleButton.IsChecked = false;
                price.houseType = "HOUSE";
            }
            if (ApartmentToggleButton.IsChecked == true)
            {
                HouseToggleButton.IsChecked = false;
                RowHouseToggleButton.IsChecked = false;
                PairHouseToggleButton.IsChecked = false;
                SummerHouseToggleButton.IsChecked = false;
                SaunaToggleButton.IsChecked = false;
                price.houseType = "APARTMENT";
            }
            if (RowHouseToggleButton.IsChecked == true)
            {
                HouseToggleButton.IsChecked = false;
                ApartmentToggleButton.IsChecked = false;
                PairHouseToggleButton.IsChecked = false;
                SummerHouseToggleButton.IsChecked = false;
                SaunaToggleButton.IsChecked = false;
                price.houseType = "ROWHOUSE";
            }
            if (PairHouseToggleButton.IsChecked == true)
            {
                HouseToggleButton.IsChecked = false;
                ApartmentToggleButton.IsChecked = false;
                RowHouseToggleButton.IsChecked = false;
                SummerHouseToggleButton.IsChecked = false;
                SaunaToggleButton.IsChecked = false;
                price.houseType = "PAIRHOUSE";
            }
            if (SummerHouseToggleButton.IsChecked == true)
            {
                HouseToggleButton.IsChecked = false;
                ApartmentToggleButton.IsChecked = false;
                RowHouseToggleButton.IsChecked = false;
                PairHouseToggleButton.IsChecked = false;
                SaunaToggleButton.IsChecked = false;
                price.houseType = "SUMMERHOUSE";
            }
            if (SaunaToggleButton.IsChecked == true)
            {
                HouseToggleButton.IsChecked = false;
                ApartmentToggleButton.IsChecked = false;
                RowHouseToggleButton.IsChecked = false;
                PairHouseToggleButton.IsChecked = false;
                SummerHouseToggleButton.IsChecked = false;
                price.houseType = "SAUNA";
            }

            if (MonthToggleButton.IsChecked == true)
            {
                QuarterToggleButton.IsChecked = false;
                YearToggleButton.IsChecked = false;
                price.billingPeriod = "MONTH";
                
            }
            if (QuarterToggleButton.IsChecked == true)
            {
                MonthToggleButton.IsChecked = false;
                YearToggleButton.IsChecked = false;
                price.billingPeriod = "QUARTER";
                
            }
            if (YearToggleButton.IsChecked == true)
            {
                MonthToggleButton.IsChecked = false;
                QuarterToggleButton.IsChecked = false;
                price.billingPeriod = "YEAR";
                
            }
            if (EuroToggleButton.IsChecked == true)
            {
                DollarToggleButton.IsChecked = false;
                price.currency = "EUR";
              
            }

            if (DollarToggleButton.IsChecked == true)
            {
                EuroToggleButton.IsChecked = false;
                price.currency = "USD";
            }
            price.area = OfferAreaTextBox.Text;
            price.buildYear = OfferBuildYearTextBox.Text;
            price.address = OfferAddressTextBox.Text;
            price.postalCode = OfferAddressTextBox.Text;
            price.insuranceStartDate = OfferStartDatePicker.Date.Day.ToString() + "." + OfferStartDatePicker.Date.Month.ToString() + "." + OfferStartDatePicker.Date.Year.ToString();
            Debug.WriteLine(price.insuranceStartDate);
            //TÄHÄN LASKEMINEN
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Price));
            ser.WriteObject(stream1, price);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            string sr_string = sr.ReadToEnd();
            Debug.WriteLine(sr_string);
            string RequestUrl = "http://185.20.136.51/sellertool/prices/";

            HttpClient clientOb = new HttpClient();
            string plain = "LUT" + ":" + "0gmsl48hgi_jhfiud76";
            string authString = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
            clientOb.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
            clientOb.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await clientOb.PostAsync(RequestUrl, new StringContent(sr_string, Encoding.UTF8, "application/json"));
            using (HttpContent content = response.Content)
            {
                var result = await content.ReadAsStringAsync();
                Debug.WriteLine(result);
                ReturnedData deSer = JsonConvert.DeserializeObject<ReturnedData>(result);
                OfferPriceResultTextBox.Text = deSer.price.price.ToString();
            }

            //TÄHÄN SITTEN SE OTSON KANTAAN ÄNKEMINEN JOKA EI EES VITTU TUU TÄLLE SIVULLE LEL
            

            
        }

       


    }
}
