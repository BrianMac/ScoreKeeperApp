using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioPlayEx;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace ScoreKeeper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Timers : ContentPage
    {

        public Timers()
        {
            InitializeComponent();
            SetScaleAndTranslationDefaults();
        }

        int timerLenghtSeconds = 0;
        int timerLenghtMinutes = 1;
        int timerLenghthHours = 0;
        int timeUnits;

        public void SetScaleAndTranslationDefaults()
        {
            shapey.Scale = 4.2;
            top_sand.TranslationY = 0;
            top_sand.Scale = 1;
            sand.ScaleX = 0;
            sand.ScaleY = 1;
            bottom_sand.Opacity = 0;
            bottom_sand.ScaleX = 1;
            bottom_sand.ScaleY = 0.1;
            bottom_sand.TranslationY = 100;
            sand_b.ScaleY = 0.5;
            sand_b.TranslationY = -63;
            sand_c.ScaleY = 0.30;
            sand_c.ScaleX = 0.55;
            sand_c.TranslationY = 138;
        }

        public void StartGameTimer(object sender, EventArgs e)
        {
            StartTimerAnimation();
        }

        public void CalcTimespan()
        {
            int seconds = (timerLenghthHours * 3600) + (timerLenghtMinutes * 60) + timerLenghtSeconds;
            seconds = seconds > 5 ? seconds - 2 : seconds;
            timeUnits = Convert.ToInt32(seconds / 5);
        }

        public async void StartTimerAnimation()
        {
            SetScaleAndTranslationDefaults();
            CalcTimespan();

            // Animation stage 1: 1 time segment
            bottom_sand.Opacity = 1;
            var ts1 = top_sand.TranslateTo(0, 8, 1000, Easing.Linear);
            var s1 = sand.ScaleXTo(0.1, 1000, Easing.SinIn);
            var s2 = sand.TranslateTo(0, 8, 1000, Easing.Linear);
            var sb1 = sand_b.TranslateTo(0, -58, 500, Easing.Linear);
            var bs1 = bottom_sand.ScaleYTo(1, 1000, Easing.Linear);
            var bs2 = bottom_sand.TranslateTo(0, 0, 1000, Easing.Linear);
            var sc1 = sand_c.TranslateTo(0, 130, 1000, Easing.Linear);

                await Task.WhenAll(ts1, s1, s2, sb1, bs1, bs2, sc1);

            // Animation stage 2: 3 time segments
            var s3 = sand.ScaleXTo(1, Convert.ToUInt32(1000 * timeUnits), Easing.Linear);
            var s4 = sand.TranslateTo(0, 41, Convert.ToUInt32(3000 * timeUnits), Easing.Linear);
            var sb3 = sand_b.ScaleYTo(0, Convert.ToUInt32(3000 * timeUnits), Easing.Linear);
            var ts2 = top_sand.TranslateTo(0, 32, Convert.ToUInt32(3000 * timeUnits), Easing.Linear);
            var bs3 = bottom_sand.TranslateTo(0, -32, Convert.ToUInt32(3000 * timeUnits), Easing.Linear);
            var sc2 = sand_c.TranslateTo(0, 100, Convert.ToUInt32(3000 * timeUnits), Easing.Linear);

                await Task.WhenAll(s3, s4, sb3, bs3, ts2, sc2);

            // Animation stage 3: 2 time segments
            var s5 = sand.TranslateTo(0, 80, Convert.ToUInt32(2000 * timeUnits), Easing.Linear);
            var s6 = sand.ScaleXTo(0.1, Convert.ToUInt32(2000 * timeUnits), Easing.SinIn);
            var ts3 = top_sand.ScaleTo(0.15, Convert.ToUInt32(2000 * timeUnits), Easing.Linear);
            var ts4 = top_sand.TranslateTo(0, -2, Convert.ToUInt32(2000 * timeUnits), Easing.Linear);
            var bs4 = bottom_sand.TranslateTo(0, -45, Convert.ToUInt32(2000 * timeUnits), Easing.Linear);
            var sc3 = sand_c.TranslateTo(0, 87, Convert.ToUInt32(2000 * timeUnits), Easing.Linear);

                await Task.WhenAll(s5, s6, bs4, ts3, ts4, sc3);

            // Animation stage 2: 1 time segment
            var s7 = sand.ScaleXTo(0, 200, Easing.SinOut);
            var ts5 = top_sand.ScaleTo(0, 0, Easing.Linear);
            var bs5 = bottom_sand.TranslateTo(0, -48, 1000, Easing.Linear);
            var sc4 = sand_c.TranslateTo(0, 82, 1000, Easing.Linear);

                await Task.WhenAll(s7, ts5, bs5, sc4);

            PlayTimerSound();
            await shapey.ScaleTo(5, 200, Easing.SinOut);
            await shapey.ScaleTo(4, 400, Easing.SinIn);
            await shapey.ScaleTo(5, 600, Easing.SinOut);
            await shapey.ScaleTo(4, 800, Easing.SinIn);
            await shapey.ScaleTo(5, 1200, Easing.SinOut);
            await shapey.ScaleTo(4.2, 1400, Easing.SinIn);
        }


        private void PlayTimerSound()
        {
            DependencyService.Get<IAudio>().PlayAudioFile("timer_alarm.mp3");

            try
            {
                Vibration.Vibrate();
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
    }
}