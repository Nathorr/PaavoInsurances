using System;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZXing;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PaavoInsurances
{
    public sealed partial class CameraPage : Page
    {
        public MediaCapture _mediaCapture = new MediaCapture();
        private Result _result;
        public string navigatedFrom = "";

        public CameraPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            navigatedFrom = e.Parameter.ToString();

            try
            {
                Debug.WriteLine("Navigated");
                var cameras = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                if (cameras.Count < 1)
                {
                    //Error.Text = "No camera found, decoding static image";
                    await DecodeStaticResource();
                    return;
                }
                MediaCaptureInitializationSettings settings;

                /* DeviceInformation backWebcam = (from webcam in webcamList
                                    where webcam.EnclosureLocation != null
                                    && webcam.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back
                                    select webcam).FirstOrDefault();*/

                settings = null;//new MediaCaptureInitializationSettings { VideoDeviceId = cameras[0].Id }; // 0 => front, 1 => back

                if (cameras.Count == 1)
                {
                    settings = new MediaCaptureInitializationSettings { VideoDeviceId = cameras[0].Id }; // 0 => front, 1 => back

                }
                else
                {
                    for (int i = 0; i <= cameras.Count; i++)
                    {
                        if (cameras[i].EnclosureLocation != null && cameras[i].EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back)
                        {
                            settings = new MediaCaptureInitializationSettings { VideoDeviceId = cameras[i].Id }; // 0 => front, 1 => back
                            Debug.WriteLine("Got i" + i);
                            break;
                        }
                        else
                        {
                            settings = new MediaCaptureInitializationSettings { VideoDeviceId = cameras[0].Id }; // 0 => front, 1 => back
                            Debug.WriteLine("Got noting");
                        }
                    }


                }
                /*// First need to find all webcams
              DeviceInformationCollection webcamList = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

               DeviceInformation backWebcam = (from webcam in webcamList
                                               where webcam.EnclosureLocation != null
                                               && webcam.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back
                                               select webcam).FirstOrDefault();

               if (backWebcam == null)
               {
                   backWebcam = webcamList.Last();
               }
   */

                await _mediaCapture.InitializeAsync(settings);
                Debug.WriteLine("Init1");
                VideoCapture.Source = _mediaCapture;
                Debug.WriteLine("Init2");

                await _mediaCapture.StartPreviewAsync();
                await Task.Delay(5000);

                while (_result == null)
                {
                    Debug.WriteLine("Init3");
                    var photoStorageFile = await KnownFolders.PicturesLibrary.CreateFileAsync("scan.jpg", CreationCollisionOption.GenerateUniqueName);
                    Debug.WriteLine("Init4");

                    await _mediaCapture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), photoStorageFile);

                    Debug.WriteLine("Init5");

                    var stream = await photoStorageFile.OpenReadAsync();
                    Debug.WriteLine("source");
                    // initialize with 1,1 to get the current size of the image
                    var writeableBmp = new WriteableBitmap(1, 1);
                    writeableBmp.SetSource(stream);
                    // and create it again because otherwise the WB isn't fully initialized and decoding
                    // results in a IndexOutOfRange
                    writeableBmp = new WriteableBitmap(writeableBmp.PixelWidth, writeableBmp.PixelHeight);
                    stream.Seek(0);
                    writeableBmp.SetSource(stream);
                    Debug.WriteLine("preview");

                    _result = ScanBitmap(writeableBmp);

                    await photoStorageFile.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    Debug.WriteLine("Delete");

                }

                await _mediaCapture.StopPreviewAsync();
                Debug.WriteLine("stop");
                VideoCapture.Visibility = Visibility.Collapsed;
                CaptureImage.Visibility = Visibility.Visible;
                ScanResult.Text = _result.Text;
            }
            catch (Exception ex)
            {
                //Error.Text = ex.Message;
            }
        }

        private async System.Threading.Tasks.Task DecodeStaticResource()
        {
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"Assets\1.jpg");
            var stream = await file.OpenReadAsync();
            // initialize with 1,1 to get the current size of the image
            var writeableBmp = new WriteableBitmap(1, 1);
            writeableBmp.SetSource(stream);
            // and create it again because otherwise the WB isn't fully initialized and decoding
            // results in a IndexOutOfRange
            writeableBmp = new WriteableBitmap(writeableBmp.PixelWidth, writeableBmp.PixelHeight);
            stream.Seek(0);
            writeableBmp.SetSource(stream);
            CaptureImage.Source = writeableBmp;
            VideoCapture.Visibility = Visibility.Collapsed;
            CaptureImage.Visibility = Visibility.Visible;

            _result = ScanBitmap(writeableBmp);
            if (_result != null)
            {
                ScanResult.Text += _result.Text;
            }
            return;
        }

        private Result ScanBitmap(WriteableBitmap writeableBmp)
        {
            var barcodeReader = new BarcodeReader
            {
                TryHarder = true,
                AutoRotate = true
            };
            var result = barcodeReader.Decode(writeableBmp);

            if (result != null)
            {
                CaptureImage.Source = writeableBmp;
            }

            return result;
        }

        /* protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
         {
         
         }*/

        private void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //await _mediaCapture.StopPreviewAsync();
            //this.Frame.Navigate(typeof(CameraPage));
            if(navigatedFrom == "homePage")
            {
                this.Frame.Navigate(typeof(ScannedOldCustomerInfo), _result.Text);
            }
            else if(navigatedFrom == "scannedOldCustomerInfoPage")
            {
                this.Frame.Navigate(typeof(BonusConfirmationPage), _result.Text);
            }
        }
    }
}
