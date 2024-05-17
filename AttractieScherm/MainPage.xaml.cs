using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace H5_Weekcheck_AttractieScherm
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Maakt de hele stackpanel onzichtbaar zodra je FilePicker opent
            spAttractie.Visibility = Visibility.Collapsed;

            //Stelt de file-picker in en opent deze
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.Downloads;
            picker.FileTypeFilter.Add(".attrinfo");

            var file = await picker.PickSingleFileAsync();

            if (file == null)
            {
                tbFileInfo.Text = "Geen geldig bestand gekozen!";
                return;
            }

            tbFileInfo.Text = file.Path;

            // Open het gekozen bestand en krijg toegang om te lezen (read)
            using (var fileAccess = await file.OpenReadAsync())
            {
                // Open een Stream (soort tunneltje) van het bestand naar de lezer
                using (var stream = fileAccess.AsStreamForRead())
                {
                    // Open een 'lezer' (reader) die via de stream het bestand gaat lezen
                    using (var reader = new StreamReader(stream))
                    {
                       string imageUrl = reader.ReadLine();
                        imgAttractie.Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute));


                        spAttractie.Visibility = Visibility.Visible;
                        attractieNaam.Text = reader.ReadLine();
                        themaGebied.Text = reader.ReadLine();
                        beschrijving.Text = reader.ReadLine();
                        minLengte.Text ="Minimale lengte: " + reader.ReadLine();
                        string fastPass = reader.ReadLine();
                        if(fastPass == "Ja")
                        {
                            fastPassBox.Visibility = Visibility.Visible;
                        }
                        else if (fastPass != "Ja")
                        {
                            fastPassBox.Visibility= Visibility.Collapsed;
                        }    
                    }
                }
            }

        }
    }
}
