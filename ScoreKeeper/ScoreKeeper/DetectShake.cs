using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ScoreKeeper.Views
{
    public class DetectShake
    {
        // Set speed delay for monitoring changes.
        SensorSpeed speed = SensorSpeed.Game;

        public DetectShake()
        {
            // Register for reading changes, be sure to unsubscribe when finished
            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
        }

        async void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            // Process shake event
            await App.Current.MainPage.DisplayAlert("Alert", "Shaken!", "OK");
        }


        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                    Accelerometer.Stop();
                else
                    Accelerometer.Start(speed);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                ShowPopup("Shake detection is unsupported on this device:  " + fnsEx.Message);
            }
            catch (Exception ex)
            {
                ShowPopup("An unknown error occured:  " + ex.Message);
            }
        }

        public async void ShowPopup(string msg)
        {
            await App.Current.MainPage.DisplayAlert("Alert:", msg, "Dismiss");
        }
    }
}
