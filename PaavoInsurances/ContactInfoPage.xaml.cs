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
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;



namespace PaavoInsurances
{

    public sealed partial class ContactInfoPage : Page
    {

        public List<HomeInsuranceClass> insuranceList = new List<HomeInsuranceClass>();

        [DataContract]
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
            public InsurancePrice returnPrice;

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

        [DataContract]
        public class HomeInsuranceClass
        {
            [DataMember(Name = "pricingParameters")]
            public PricingParameters pricingParameters = new PricingParameters();
            [DataMember(Name = "id")]
            public string id { get; set; }
            [DataMember(Name = "name")]
            public string name { get; set; }
            [DataMember(Name = "surName")]
            public string surName { get; set; }
            [DataMember(Name = "validTo")]
            public string validTo { get; set; }

        }
        [DataContract]
        public class PricingParameters
        {
            [DataMember(Name = "price")]
            public InsuranceValue price = new InsuranceValue();
            [DataMember(Name = "postalCode")]
            public string postalCode { get; set; }
            [DataMember(Name = "address")]
            public string address { get; set; }
            [DataMember(Name = "area")]
            public string area { get; set; }
            [DataMember(Name = "buildYear")]
            public string buildYear { get; set; }
            [DataMember(Name = "insuranceStartDate")]
            public string insuranceStartDate { get; set; }
            [DataMember(Name = "houseType")]
            public string houseType { get; set; }
            [DataMember(Name = "currency")]
            public string currency { get; set; }
            [DataMember(Name = "billingperiod")]
            public string billingPeriod { get; set; }

        }
        [DataContract]
        public class InsuranceValue
        {
            [DataMember(Name = "price")]
            public string price { get; set; }
            [DataMember(Name = "currency")]
            public string currency { get; set; }
            [DataMember(Name = "billingPeriod")]
            public string billingPeriod { get; set; }
        }

        public ContactInfoPage()
        {
            this.InitializeComponent();
        }

        public class ContactInfoObject
        {
            public bool home { get; set; }
            public bool health { get; set; }
            public bool vehicle { get; set; }
            public bool life { get; set; }
            public bool infant { get; set; }
            public bool pets { get; set; }
        }

        /*async private void Button_Click(object sender, RoutedEventArgs e)
        {
            Price price = new Price();

            price.postalCode = 00100;
            price.address = "Mannerheimintie 50";
            price.area = 79.5f;
            price.buildYear = 1928;
            price.insuranceStartDate = "12.12.2016";
            price.houseType = "APARTMENT";
            price.currency = "EUR";
            price.billingPeriod = "YEAR";



            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Price));
            ser.WriteObject(stream1, price);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            string sr_string = sr.ReadToEnd();

            string RequestUrl = "http://185.20.136.51/sellertool/prices/";

            HttpClient clientOb = new HttpClient();
            string plain = "LUT" + ":" + "0gmsl48hgi_jhfiud76";
            string authString = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
            clientOb.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
            clientOb.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await clientOb.PostAsync(RequestUrl, new StringContent(sr_string, Encoding.UTF8, "application/json"));
            Debug.WriteLine(response);
            using (HttpContent content = response.Content)
            {
                var result = await content.ReadAsStreamAsync();
                Debug.WriteLine(result);
                DataContractJsonSerializer deSer = new DataContractJsonSerializer(typeof(ReturnedData));
                ReturnedData obj = (ReturnedData)deSer.ReadObject(result);
                Debug.WriteLine(obj.returnPrice.price);
                TextBlock resultbox = (TextBlock)this.FindName("resultbox");
                resultbox.Text = obj.returnPrice.price.ToString();
            }

        }

        async private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TextBlock resultbox = (TextBlock)this.FindName("clientInformationBox");
            resultbox.Text = "";
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
                Debug.WriteLine(deSer[1].pricingParameters.address);
                for (int i = 0; i < deSer.Count; i++)
                {
                    Debug.WriteLine(deSer[i].id);
                    if (nameInputBox.Text == deSer[i].surName)
                    {
                        resultbox.Text = deSer[i].name.ToString() + " " + deSer[i].pricingParameters.address.ToString();

                    }
                }
            }
        }*/

        private async void FillDataBase(ContactInfoObject contactInfo)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("SpamTable");

            SpamTable table = new SpamTable
            {
                name = ContactNameTextBox.Text,
                email = ContactEmailTextBox.Text,
                phone = ContactPhoneTextBox.Text,
                home = contactInfo.home,
                health = contactInfo.health,
                vehicle = contactInfo.vehicle,
                life = contactInfo.life,
                infant = contactInfo.infant,
                pets = contactInfo.pets
            };
            await conn.InsertAsync(table);
        }

        private async void FetchData()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("SpamTable");

            var query = conn.Table<SpamTable>();
            var result = await query.ToListAsync();
            DatabaseList.Text = "Name\tEmail\tPhone\tHome\tHealth\tVehicle\tLife\tInfant\tPets\n\n";
            foreach (var item in result)
            {
                DatabaseList.Text += item.name + "\t" + item.email + "\t" + item.phone + "\t" + item.home + "\t" + item.health + "\t" + item.vehicle + "\t" + item.life + "\t" + item.infant + "\t" + item.pets + "\n";
            }
        }

        private void ArrowForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmPopup.IsOpen == false)
                ConfirmPopup.IsOpen = true;
        }

        private void ConfirmationYesButton_Click(object sender, RoutedEventArgs e)
        {
            ContactInfoObject contactInfo = new ContactInfoObject();
            if (HomeToggleButton.IsChecked == true)
                contactInfo.home = true;
            else
                contactInfo.home = false;
            if (HealthToggleButton.IsChecked == true)
                contactInfo.health = true;
            else
                contactInfo.health = false;
            if (VehicleToggleButton.IsChecked == true)
                contactInfo.vehicle = true;
            else
                contactInfo.vehicle = false;
            if (PetsToggleButton.IsChecked == true)
                contactInfo.pets = true;
            else
                contactInfo.pets = false;
            if (LifeToggleButton.IsChecked == true)
                contactInfo.life = true;
            else
                contactInfo.life = false;
            if (InfantToggleButton.IsChecked == true)
                contactInfo.infant = true;
            else
                contactInfo.infant = false;
            FillDataBase(contactInfo);
            ContactInfoPopup.IsOpen = true;
            FetchData();
        }

        private void ConfirmationNoButton_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmPopup.IsOpen == true)
                ConfirmPopup.IsOpen = false;
        }

        private void ArrowBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
