using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ScoreKeeper.Views
{
    public class DetectShake
    {
        // Set speed delay for monitoring changes.
        readonly SensorSpeed speed = SensorSpeed.Game;
        DicePage DP = new DicePage();

        public DetectShake()
        {
            // Register for reading changes, be sure to unsubscribe when finished
            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
        }

        async void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            // Process shake event
            //ShowPopup("Test");            
            DP.RollDice();

        }

        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                {
                    Accelerometer.Stop();
                    ShowPopup("Shake detection is off");
                }
                else
                {
                    Accelerometer.Start(speed);
                    ShowPopup("Shake detection is on");
                }
                    
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

        public void ShowPopup(string msg)
        {
            App.Current.MainPage.DisplayAlert("Alert:", msg, "Dismiss");
        }
    }
}
