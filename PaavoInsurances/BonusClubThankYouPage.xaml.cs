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
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PaavoInsurances
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BonusClubThankYouPage : Page
    {
        public BonusClubThankYouPage()
        {
            this.InitializeComponent();
            ChangePage();
        }

        public async void ChangePage()
        {
            await Task.Delay(5000);
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
