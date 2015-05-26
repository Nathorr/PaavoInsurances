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



namespace PaavoInsurances
{
   
    public sealed partial class ContactInfoPage : Page
    {
        [DataContract]
        public class ReturnedData
        {
            [DataMember(Name="postalCode")]
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

        }

        public ContactInfoPage()
        {
            this.InitializeComponent();
        }

        async private void Button_Click(object sender, RoutedEventArgs e)
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
    }
}
