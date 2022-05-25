using System;
using System.Threading.Tasks;
using AudioPlayEx;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Timers;


namespace ScoreKeeper.Views 
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class Timers : ContentPage
    {
        public static Timer tmr = new Timer(1000);

        public Timers()
        {
            InitializeComponent();
            SetScaleAndTranslationDefaults();
            tmr.Elapsed += Tmr_Elapsed;
            tmr.Enabled = true;
            tmr.AutoReset = true;
        }

        bool IsTimerRunning = false;
        bool playSound = true;
        public double secondHandRotation = 0.0;
        public double minuteHandRotation = 0.0;
        public int timerLengthSeconds;
        public int timeRemaining;
        public string remainingTime = "00:00:00";
        
        public uint CalcTimespan()
        {
            int seconds = timerLengthSeconds;
            timeRemaining = timerLengthSeconds;
            return Convert.ToUInt32(seconds / 14);
        }

        public void SetScaleAndTranslationDefaults()
        {
            sand_fall.Opacity = 0;
            sand_fall.ScaleX = 0;
            top_sand.TranslationY = 0;
            top_sand.Scale = 1;
            bottom_sand.TranslationY = 50;
            bottom_sand.ScaleY = 0.1;
            sand_top.TranslationY = 0;
            sand_btm.TranslationY = 185;
            shapey.Scale = 4.75;
        }

        public async void StartTimerAnimation()
        {
            IsTimerRunning = true;
            SetScaleAndTranslationDefaults();
            uint timeUnits = CalcTimespan();

            // Animation stage 1: 2 time segment

            var sandFall_fd_1 = sand_fall.FadeTo(1, 300 * timeUnits, Easing.Linear);
            var sandFall_sc_1 = sand_fall.ScaleXTo(1, 300 * timeUnits, Easing.Linear);
            var topSand_tx_1 = top_sand.TranslateTo(0, 8, 2000 * timeUnits, Easing.Linear);
            var btmSand_tx_1 = bottom_sand.TranslateTo(0, -50, 2000 * timeUnits, Easing.Linear);
            var btmSand_sc_1 = bottom_sand.ScaleYTo(1, 2000 * timeUnits, Easing.Linear);
            var sandTop_tx_1 = sand_top.TranslateTo(0, 8, 2000 * timeUnits, Easing.Linear);
            var sandBtm_tx_1 = sand_btm.TranslateTo(0, 180, 2000 * timeUnits, Easing.Linear);

            await Task.WhenAll(sandFall_fd_1, sandFall_sc_1, topSand_tx_1, btmSand_tx_1, btmSand_sc_1, sandTop_tx_1, sandBtm_tx_1);

            // Animation stage 2: 6 time segments

            var topSand_tx_2 = top_sand.TranslateTo(0, 30, 6000 * timeUnits, Easing.Linear);
            var btmSand_tx_2 = bottom_sand.TranslateTo(0, -85, 6000 * timeUnits, Easing.Linear);
            var sandTop_tx_2 = sand_top.TranslateTo(0, 30, 6000 * timeUnits, Easing.Linear);
            var sandBtm_tx_2 = sand_btm.TranslateTo(0, 144.5, 6000 * timeUnits, Easing.Linear);

            await Task.WhenAll(topSand_tx_2, btmSand_tx_2, sandTop_tx_2, sandBtm_tx_2);

            // Animation stage 3: 5 time segments

            var topSand_sc_1 = top_sand.ScaleTo(0.1, 5000 * timeUnits, Easing.Linear);
            var topSand_tx_3 = top_sand.TranslateTo(0, 2, 5000 * timeUnits, Easing.Linear);
            var btmSand_tx_3 = bottom_sand.TranslateTo(0, -100, 5000 * timeUnits, Easing.Linear);
            var sandTop_tx_3 = sand_top.TranslateTo(0, 72.4, 5000 * timeUnits, Easing.Linear);
            var sandBtm_tx_3 = sand_btm.TranslateTo(0, 129, 5000 * timeUnits, Easing.Linear);

            await Task.WhenAll(topSand_sc_1, topSand_tx_3, btmSand_tx_3, sandTop_tx_3, sandBtm_tx_3);

            // Animation stage 2: 1 time segment

            var topSand_sc_2 = top_sand.ScaleTo(0, 300 * timeUnits, Easing.Linear);
            var sandFall_fd_2 = sand_fall.FadeTo(0, 1000 * timeUnits, Easing.Linear);
            var sandFall_sc_2 = sand_fall.ScaleXTo(0.0, 800 * timeUnits, Easing.Linear);

            await Task.WhenAll(topSand_sc_2, sandFall_fd_2, sandFall_sc_2);
    }

        public async void StartStopwatchAnimation()
        {
            //tmr.Start();
            uint timeUnits = CalcTimespan();
            var secondHand = clock_seconds.RotateTo(0, 14000 * timeUnits, Easing.Linear);
            var minuteHand = clock_minutes.RotateTo(0, 14000 * timeUnits, Easing.Linear);
            await Task.WhenAll(secondHand, minuteHand);
            PlayTimerSound();
            CancelTimerBtn_Clicked();
        }

        private void PlayTimerSound()
        {
            if (playSound)
            {
                DependencyService.Get<IAudio>().PlayAudioFile("timer_alarm.mp3");
                playSound = false;
                try
                {
                    Vibration.Vibrate();
                }
                catch (FeatureNotSupportedException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void Tmr_Elapsed(object sender, ElapsedEventArgs e)
        {
            //if (timeRemaining > 0)
            //{
            //    timeRemaining -= 1;
            //    TimeSpan time = TimeSpan.FromSeconds(timeRemaining);
            //    remainingTime = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
            //}
            //else
            //{
            //    tmr.Stop();
            //    SetScaleAndTranslationDefaults();
            //    CancelTimerBtn_Clicked();
            //};
        }

        private void SliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            clock_seconds.Rotation = e.NewValue * 21600;
            clock_minutes.Rotation = e.NewValue * 360;
            timerLengthSeconds = Convert.ToInt32(e.NewValue * 3600);
            TimeSpan time = TimeSpan.FromSeconds(timerLengthSeconds);
            timeremaining.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
            time_remain_label.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
            if (e.NewValue != 0.0)
            {
                default_frame.IsVisible = false;
                selected_frame.IsVisible = true;
                btn30sec.BackgroundColor = Color.FromHex("#483d8b");
                btn1min.BackgroundColor = Color.FromHex("#483d8b");
                btn5min.BackgroundColor = Color.FromHex("#483d8b");
                btn10min.BackgroundColor = Color.FromHex("#483d8b");
                StartTimerBtn.BackgroundColor = Color.FromHex("#0072b1");
                StartTimerBtn.TextColor = Color.FromHex("#ffffff");
            }
            else
            {
                selected_frame.IsVisible = false;
                default_frame.IsVisible = true;
                StartTimerBtn.BackgroundColor = Color.FromHex("#005482");
                StartTimerBtn.TextColor = Color.FromHex("#ebebeb");
            }
        }

        private void SetTimeRemaining(object sender, EventArgs e)
        {
            Button clicked = sender as Button;
            btn30sec.BackgroundColor = Color.FromHex("#483d8b");
            btn1min.BackgroundColor = Color.FromHex("#483d8b");
            btn5min.BackgroundColor = Color.FromHex("#483d8b");
            btn10min.BackgroundColor = Color.FromHex("#483d8b");
            StartTimerBtn.BackgroundColor = Color.FromHex("#0072b1");
            StartTimerBtn.TextColor = Color.FromHex("#ffffff");
            selected_frame.IsVisible = true;
            default_frame.IsVisible = false;
            if (selected_frame.IsVisible)
            {
                selected_frame.IsVisible = false;
                default_frame.IsVisible = true;
                time_slider.Value = 0.0;
            }
            
            switch (clicked.ClassId)
            {
                case ("30sec"):
                    timerLengthSeconds = 30;
                    clock_seconds.Rotation = 180;
                    clock_minutes.Rotation = 1.5;
                    btn30sec.BackgroundColor = Color.FromHex("#6959c9");
                    break;
                case ("1min"):
                    timerLengthSeconds = 60;
                    clock_seconds.Rotation = 360;
                    clock_minutes.Rotation = 3;
                    btn1min.BackgroundColor = Color.FromHex("#6959c9");
                    break;
                case ("5min"):
                    timerLengthSeconds = 300;
                    clock_seconds.Rotation = 5*360;
                    clock_minutes.Rotation = 30;
                    btn5min.BackgroundColor = Color.FromHex("#6959c9");
                    break;
                default:
                    timerLengthSeconds = 600;
                    clock_seconds.Rotation = 10*360;
                    clock_minutes.Rotation = 60;
                    btn10min.BackgroundColor = Color.FromHex("#6959c9");
                    break;
            }
            TimeSpan time = TimeSpan.FromSeconds(timerLengthSeconds);
            timeremaining.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
            time_remain_label.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
        }

        public void ShowPopup(string msg)
        {
            App.Current.MainPage.DisplayAlert("Alert:", msg, "Dismiss");
        }

        public void AnimatedTimerState(string state = "set")
        {
            if (state == "reset")
            {
                ViewExtensions.CancelAnimations(top_sand);
                ViewExtensions.CancelAnimations(bottom_sand);
                ViewExtensions.CancelAnimations(sand_fall);
                ViewExtensions.CancelAnimations(sand_top);
                ViewExtensions.CancelAnimations(sand_btm);
                ViewExtensions.CancelAnimations(shapey);
                SetScaleAndTranslationDefaults();
            }
        }

        private void StopwatchState(string state = "set")
        {
            if (state == "reset")
            {
                clock_seconds.Rotation = secondHandRotation;
                clock_minutes.Rotation = minuteHandRotation;
            }
            else
            {
                secondHandRotation = clock_seconds.Rotation;
                minuteHandRotation = clock_minutes.Rotation;
            }
        }

        private void CancelTimerBtn_Clicked(object sender = null, EventArgs e = null)
        {
            StartTimerBtn.IsVisible = true;
            StartTimerBtn.IsEnabled = true;
            CancelTimerBtn.IsVisible = false;
            CancelTimerBtn.IsEnabled = false;
            btn30sec.IsEnabled = true;
            btn1min.IsEnabled = true;
            btn5min.IsEnabled = true;
            btn10min.IsEnabled = true;
            time_slider.IsEnabled = true;
            StopwatchState("reset");
            AnimatedTimerState("reset");
            SetScaleAndTranslationDefaults();
            IsTimerRunning = false;
            playSound = false;  
        }

        private void StartTimerBtn_Clicked(object sender, EventArgs e)
        {
            if (timerLengthSeconds > 0)
            {
                StopwatchState();
                timeRemaining = timerLengthSeconds;
                CancelTimerBtn.IsVisible = true;
                CancelTimerBtn.IsEnabled = true;
                StartTimerBtn.IsVisible = false;
                StartTimerBtn.IsEnabled = false;
                btn30sec.IsEnabled = false;
                btn1min.IsEnabled = false;
                btn5min.IsEnabled = false;
                btn10min.IsEnabled = false;
                time_slider.IsEnabled = false;
                StartStopwatchAnimation();
                StartTimerAnimation();
                tmr.Start();
            }
            else
            {
                ShowPopup("Please Set Timer Length.");
            }
        }
    }
}