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
            this.Frame.Navigate(typeof(SpamThankYouPage));
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
