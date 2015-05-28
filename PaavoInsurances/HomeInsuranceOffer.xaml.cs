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
        public Price price = new Price();
        public ScannedOldCustomerInfo.CameraClass cameraClass = new ScannedOldCustomerInfo.CameraClass();
        public HomeInsuranceOffer()
        {
            this.InitializeComponent();
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


        public HomeInsuranceClass homeInsurance = new HomeInsuranceClass();

        public async void CalculatePrice()
        {


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
                cameraClass.homeInsuranceClass.pricingParameters.price.price = deSer.price.price.ToString();


            }
        }

        private async void OfferPriceMeButton_Click(object sender, RoutedEventArgs e)
        {

            CalculatePrice();
            //TÄHÄN SITTEN SE OTSON KANTAAN ÄNKEMINEN JOKA EI EES VITTU TUU TÄLLE SIVULLE LEL



        }
        private void ArrowForwardButton_Click(object sender, RoutedEventArgs e)
        {
            //Price price = new Price();
            //CameraClass cameraClass = new CameraClass();

            CalculatePrice();

            if (HouseToggleButton.IsChecked == true)
            {
                ApartmentToggleButton.IsChecked = false;
                RowHouseToggleButton.IsChecked = false;
                PairHouseToggleButton.IsChecked = false;
                SummerHouseToggleButton.IsChecked = false;
                SaunaToggleButton.IsChecked = false;
                //price.houseType = "HOUSE";
                cameraClass.homeInsuranceClass.pricingParameters.houseType = "HOUSE";
            }
            if (ApartmentToggleButton.IsChecked == true)
            {
                HouseToggleButton.IsChecked = false;
                RowHouseToggleButton.IsChecked = false;
                PairHouseToggleButton.IsChecked = false;
                SummerHouseToggleButton.IsChecked = false;
                SaunaToggleButton.IsChecked = false;
                cameraClass.homeInsuranceClass.pricingParameters.houseType = "APARTMENT";
            }
            if (RowHouseToggleButton.IsChecked == true)
            {
                HouseToggleButton.IsChecked = false;
                ApartmentToggleButton.IsChecked = false;
                PairHouseToggleButton.IsChecked = false;
                SummerHouseToggleButton.IsChecked = false;
                SaunaToggleButton.IsChecked = false;
                cameraClass.homeInsuranceClass.pricingParameters.houseType = "ROWHOUSE";
            }
            if (PairHouseToggleButton.IsChecked == true)
            {
                HouseToggleButton.IsChecked = false;
                ApartmentToggleButton.IsChecked = false;
                RowHouseToggleButton.IsChecked = false;
                SummerHouseToggleButton.IsChecked = false;
                SaunaToggleButton.IsChecked = false;
                cameraClass.homeInsuranceClass.pricingParameters.houseType = "PAIRHOUSE";
            }
            if (SummerHouseToggleButton.IsChecked == true)
            {
                HouseToggleButton.IsChecked = false;
                ApartmentToggleButton.IsChecked = false;
                RowHouseToggleButton.IsChecked = false;
                PairHouseToggleButton.IsChecked = false;
                SaunaToggleButton.IsChecked = false;
                cameraClass.homeInsuranceClass.pricingParameters.houseType = "SUMMERHOUSE";
            }
            if (SaunaToggleButton.IsChecked == true)
            {
                HouseToggleButton.IsChecked = false;
                ApartmentToggleButton.IsChecked = false;
                RowHouseToggleButton.IsChecked = false;
                PairHouseToggleButton.IsChecked = false;
                SummerHouseToggleButton.IsChecked = false;
                cameraClass.homeInsuranceClass.pricingParameters.houseType = "SAUNA";
            }

            if (MonthToggleButton.IsChecked == true)
            {
                QuarterToggleButton.IsChecked = false;
                YearToggleButton.IsChecked = false;
                cameraClass.homeInsuranceClass.pricingParameters.price.billingPeriod = "MONTH";
                cameraClass.homeInsuranceClass.pricingParameters.billingPeriod = "MONTH";

            }
            if (QuarterToggleButton.IsChecked == true)
            {
                MonthToggleButton.IsChecked = false;
                YearToggleButton.IsChecked = false;
                cameraClass.homeInsuranceClass.pricingParameters.price.billingPeriod = "QUARTER";
                cameraClass.homeInsuranceClass.pricingParameters.billingPeriod = "QUARTER";

            }
            if (YearToggleButton.IsChecked == true)
            {
                MonthToggleButton.IsChecked = false;
                QuarterToggleButton.IsChecked = false;
                cameraClass.homeInsuranceClass.pricingParameters.price.billingPeriod = "YEAR";
                cameraClass.homeInsuranceClass.pricingParameters.billingPeriod = "YEAR";

            }
            if (EuroToggleButton.IsChecked == true)
            {
                DollarToggleButton.IsChecked = false;
                cameraClass.homeInsuranceClass.pricingParameters.price.currency = "EUR";
                cameraClass.homeInsuranceClass.pricingParameters.currency = "EUR";

            }

            if (DollarToggleButton.IsChecked == true)
            {
                EuroToggleButton.IsChecked = false;
                cameraClass.homeInsuranceClass.pricingParameters.price.currency = "USD";
                cameraClass.homeInsuranceClass.pricingParameters.currency = "USD";
            }

            cameraClass.homeInsuranceClass.pricingParameters.area = OfferAreaTextBox.Text;
            cameraClass.homeInsuranceClass.pricingParameters.buildYear = OfferBuildYearTextBox.Text;
            cameraClass.homeInsuranceClass.pricingParameters.address = OfferAddressTextBox.Text;
            cameraClass.homeInsuranceClass.pricingParameters.postalCode = OfferAddressTextBox.Text;
            cameraClass.homeInsuranceClass.pricingParameters.insuranceStartDate = OfferStartDatePicker.Date.Day.ToString() + "." + OfferStartDatePicker.Date.Month.ToString() + "." + OfferStartDatePicker.Date.Year.ToString();
            cameraClass.previousPage = "homeInsuranceOffer";

            this.Frame.Navigate(typeof(CameraPage), cameraClass);


        }

        private void ArrowBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void HouseToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ApartmentToggleButton.IsChecked = false;
            RowHouseToggleButton.IsChecked = false;
            PairHouseToggleButton.IsChecked = false;
            SummerHouseToggleButton.IsChecked = false;
            SaunaToggleButton.IsChecked = false;
        }

        private void ApartmentToggleButton_Click(object sender, RoutedEventArgs e)
        {
            HouseToggleButton.IsChecked = false;
            RowHouseToggleButton.IsChecked = false;
            PairHouseToggleButton.IsChecked = false;
            SummerHouseToggleButton.IsChecked = false;
            SaunaToggleButton.IsChecked = false;
        }

        private void RowHouseToggleButton_Click(object sender, RoutedEventArgs e)
        {
            HouseToggleButton.IsChecked = false;
            ApartmentToggleButton.IsChecked = false;
            PairHouseToggleButton.IsChecked = false;
            SummerHouseToggleButton.IsChecked = false;
            SaunaToggleButton.IsChecked = false;
        }

        private void PairHouseToggleButton_Click(object sender, RoutedEventArgs e)
        {
            HouseToggleButton.IsChecked = false;
            ApartmentToggleButton.IsChecked = false;
            RowHouseToggleButton.IsChecked = false;
            SummerHouseToggleButton.IsChecked = false;
            SaunaToggleButton.IsChecked = false;
        }

        private void SummerHouseToggleButton_Click(object sender, RoutedEventArgs e)
        {
            HouseToggleButton.IsChecked = false;
            ApartmentToggleButton.IsChecked = false;
            RowHouseToggleButton.IsChecked = false;
            PairHouseToggleButton.IsChecked = false;
            SaunaToggleButton.IsChecked = false;
        }

        private void SaunaToggleButton_Click(object sender, RoutedEventArgs e)
        {
            HouseToggleButton.IsChecked = false;
            ApartmentToggleButton.IsChecked = false;
            RowHouseToggleButton.IsChecked = false;
            PairHouseToggleButton.IsChecked = false;
            SummerHouseToggleButton.IsChecked = false;
        }

        private void MonthToggleButton_Click(object sender, RoutedEventArgs e)
        {
            YearToggleButton.IsChecked = false;
            QuarterToggleButton.IsChecked = false;
        }

        private void QuarterToggleButton_Click(object sender, RoutedEventArgs e)
        {
            YearToggleButton.IsChecked = false;
            MonthToggleButton.IsChecked = false;
        }

        private void YearToggleButton_Click(object sender, RoutedEventArgs e)
        {
            QuarterToggleButton.IsChecked = false;
            MonthToggleButton.IsChecked = false;
        }

        private void EuroToggleButton_Click(object sender, RoutedEventArgs e)
        {
            DollarToggleButton.IsChecked = false;
        }

        private void DollarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            EuroToggleButton.IsChecked = false;
        }

        private void buildYearKeyDown(object sender, KeyRoutedEventArgs e)
        {
            char key = Convert.ToChar(e.Key);

            if (key < '0' || key > '9')
            {
                e.Handled = true;
            }
        }

        private void postalCodeKeyDown(object sender, KeyRoutedEventArgs e)
        {
            char key = Convert.ToChar(e.Key);

            if (key < '0' || key > '9')
            {
                e.Handled = true;
            }
        }
    }
}