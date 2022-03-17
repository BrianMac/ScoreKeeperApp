using AudioPlayEx;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ScoreKeeper.Models;

namespace ScoreKeeper.Views
{
    public partial class DicePage : ContentPage
    {
        #region GlobalVariables

        readonly SensorSpeed speed = SensorSpeed.Game;
        int numD100 = 0;
        int numD20 = 0;
        int numD6 = 0;
        int numD4 = 0;
        int numDx = 0;

        //int Dxlow = Convert.ToInt32(Application.Current.Properties["LowEndNumber"]);
        //int Dxhigh = Convert.ToInt32(Application.Current.Properties["HighEndNumber"]);
        public static int Dxlow = 1;
        public static int Dxhigh = 50;
        public static int Range = ((Dxlow - 1) - Dxhigh);

        int resultsTotal = 0;
        int resultsLastTotal = 0;
        bool rollSoundEnabled = true;

        #endregion

        public DicePage()
        {
            InitializeComponent();
            DetectShake();
        }

        public void RollDice(object sender, EventArgs e)
        {
            if (numD100 > 0)
                RotateDiceImages("D100");

            if (numD20 > 0)
                RotateDiceImages("D20");

            if (numD6 > 0)
                RotateDiceImages("D6");

            if (numD4 > 0)
                RotateDiceImages("D4");

            if (numDx > 0)
                RotateDiceImages("Dx");

            if (rollSoundEnabled)
            {
                DependencyService.Get<IAudio>().PlayAudioFile("dice_roll.mp3");
            }


            int[] SelectedDice = new int[] { numD100, numD20, numD6, numD4, numDx };
            ShowRollResults(GenerateDiceRolls(SelectedDice));
            RollButton.Text = "REROLL DICE";
        }

        private List<int[]> GenerateDiceRolls(int[] selectedDice)
        {
            List<int[]> DiceRollsList = new List<int[]>();
            for (int i = 0; i < selectedDice.Length; i++)
            {
                int[] rollArray = new int[selectedDice[i]];
                for (int j = 0; j < selectedDice[i]; j++)
                {
                    Random rnd = new Random();
                    if (i == 0) //D100
                        rollArray[j] = rnd.Next(1, 101);

                    if (i == 1) //D20
                        rollArray[j] = rnd.Next(1, 21);

                    if (i == 2) //D6
                        rollArray[j] = rnd.Next(1, 7);

                    if (i == 3) //D4
                        rollArray[j] = rnd.Next(1, 5);

                    if (i == 4)
                    {
                        // Dxlow = Convert.ToInt32(Application.Current.Properties["LowEndNumber"]);
                        // Dxhigh = Convert.ToInt32(Application.Current.Properties["HighEndNumber"]);
                        rollArray[j] = rnd.Next(Dxlow, Dxhigh + 1);
                    } //Dx

                        
                }
                DiceRollsList.Add(rollArray);
            }
            return DiceRollsList;
        }

        private void SetScoreOutputLabel(int DiceIndex, int[] diceArr, int Total)
        {

            Label[] d100Array = new Label[] { ResultsD100A, ResultsD100B, ResultsD100Equals, ResultsD100C };
            Label[] d20Array = new Label[] { ResultsD20A, ResultsD20B, ResultsD20Equals, ResultsD20C };
            Label[] d6Array = new Label[] { ResultsD6A, ResultsD6B, ResultsD6Equals, ResultsD6C };
            Label[] d4Array = new Label[] { ResultsD4A, ResultsD4B, ResultsD4Equals, ResultsD4C };
            Label[] dxArray = new Label[] { ResultsDxA, ResultsDxB, ResultsDxEquals, ResultsDxC };
            Label[][] outputRows = new Label[][] { d100Array, d20Array, d6Array, d4Array, dxArray };
            string[] dice = new string[] { "D100", "D20", "D6", "D4", "Custom" };
            int[] numberOfDice = new int[] { numD100, numD20, numD6, numD4, numDx };

            string DiceCollection = "( ";

            if (Total > 0)
            {
                LineRow.IsVisible = true;

                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            outputRows[DiceIndex][i].Text = $"{dice[DiceIndex]} × {numberOfDice[DiceIndex]} ";
                            outputRows[DiceIndex][i].IsVisible = true;
                            break;
                        case 1:
                            for (int j = 0; j < diceArr.Length; j++)
                            {
                                if (j == diceArr.Length - 1)
                                    DiceCollection += diceArr[j].ToString() + " )";
                                else
                                    DiceCollection += diceArr[j].ToString() + ", ";
                            }
                            outputRows[DiceIndex][i].Text = DiceCollection;
                            outputRows[DiceIndex][i].IsVisible = true;
                            break;
                        case 2:
                            outputRows[DiceIndex][i].Text = " = ";
                            outputRows[DiceIndex][i].IsVisible = true;
                            break;
                        case 3:
                            outputRows[DiceIndex][i].Text = Total.ToString();
                            outputRows[DiceIndex][i].IsVisible = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    outputRows[DiceIndex][i].Text = "";
                    outputRows[DiceIndex][i].IsVisible = false;
                }
                LineRow.IsVisible = false;
            }
        }

        private void ShowRollResults(List<int[]> RollArrayList)
        {
            resultsLastTotal = resultsTotal;
            resultsTotal = 0;

            foreach (var rollArr in RollArrayList.Select((value, i) => new { i, value }))
            {
                var value = rollArr.value;
                var index = rollArr.i;
                SetScoreOutputLabel(index, value, value.Sum());
                resultsTotal += value.Sum();
            }
            ResultsLastTotal.IsVisible = true;
            TtlAmtRoll.IsVisible = true;
            ResultsTotal.IsVisible = true;
            ResultsTotal.Text = resultsTotal.ToString();
            ResultsLastTotal.Text = "Last Amount Rolled: " + resultsLastTotal.ToString();
        }

        public void ClearResults(object sender, EventArgs e)
        {
            ResultsD100A.Text = "";
            ResultsD20A.Text = "";
            ResultsD6A.Text = "";
            ResultsD4A.Text = "";
            ResultsDxA.Text = "";

            ResultsD100B.Text = "";
            ResultsD20B.Text = "";
            ResultsD6B.Text = "";
            ResultsD4B.Text = "";
            ResultsDxB.Text = "";

            ResultsD100C.Text = "";
            ResultsD20C.Text = "";
            ResultsD6C.Text = "";
            ResultsD4C.Text = "";
            ResultsDxC.Text = "";

            ResultsD100A.IsVisible = false;
            ResultsD20A.IsVisible = false;
            ResultsD6A.IsVisible = false;
            ResultsD4A.IsVisible = false;
            ResultsDxA.IsVisible = false;

            ResultsD100B.IsVisible = false;
            ResultsD20B.IsVisible = false;
            ResultsD6B.IsVisible = false;
            ResultsD4B.IsVisible = false;
            ResultsDxB.IsVisible = false;

            ResultsD100Equals.IsVisible = false;
            ResultsD20Equals.IsVisible = false;
            ResultsD6Equals.IsVisible = false;
            ResultsD4Equals.IsVisible = false;
            ResultsDxEquals.IsVisible = false;

            ResultsD100C.IsVisible = false;
            ResultsD20C.IsVisible = false;
            ResultsD6C.IsVisible = false;
            ResultsD4C.IsVisible = false;
            ResultsDxC.IsVisible = false;

            D100lbl.IsVisible = false;
            D20lbl.IsVisible = false;
            D6lbl.IsVisible = false;
            D4lbl.IsVisible = false;
            Dxlbl.IsVisible = false;

            ResultsTotal.IsVisible = false;
            ResultsTotal.Text = "";
            ResultsLastTotal.Text = "Last Amount Rolled:";

            numD100 = 0;
            numD20 = 0;
            numD6 = 0;
            numD4 = 0;
            numDx = 0;
            resultsTotal = 0;
            resultsLastTotal = 0;

            TurnOnAccelerometer(false);
            RollButton.Text = "ROLL DICE";
            RollButton.IsEnabled = false;
            TtlAmtRoll.IsVisible = false;
            LineRow.IsVisible = false;
            ResultsLastTotal.IsVisible = false;
        }

        //Todo replace speaker characters with images that match the style of app design
        public void ToggleSound(object sender, EventArgs e)
        {
            rollSoundEnabled = !rollSoundEnabled;
            if (rollSoundEnabled)
            {
                SoundButton.Text = "🔈";
            }
            else
            {
                SoundButton.Text = "🔇";
            }
        }

        public void RotateDiceImages(string die)
        {
            double rotationImg = 720;

            switch (die)
            {
                case "D100":
                    D100.Rotation = 0;
                    D100.RotateTo(rotationImg, 1000, Xamarin.Forms.Easing.CubicInOut);
                    break;

                case "D20":
                    D20.Rotation = 0;
                    D20.RotateTo(rotationImg, 1000, Xamarin.Forms.Easing.CubicInOut);
                    break;

                case "D6":
                    D6.Rotation = 0;
                    D6.RotateTo(rotationImg, 1000, Xamarin.Forms.Easing.CubicInOut);
                    break;

                case "D4":
                    D4.Rotation = 0;
                    D4.RotateTo(rotationImg, 1000, Xamarin.Forms.Easing.CubicInOut);
                    break;
                case "Dx":
                    Dx.Rotation = 0;
                    Dx.RotateTo(rotationImg, 1000, Xamarin.Forms.Easing.CubicInOut);
                    break;

                default:
                    Console.WriteLine("!");
                    break;
            }
        }

        //Todo refactor this
        public void IncrementDice(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.ClassId.ToString() == "D100PlusClass")
            {
                numD100 += 1;
                D100lbl.IsVisible = true;
                D100lbl.Text = $"100 sided die × {numD100}";
            }
            else if (btn.ClassId.ToString() == "D100MinusClass")
            {
                numD100 -= 1;
                if (numD100 <= 0)
                {
                    numD100 = 0;
                    D100lbl.IsVisible = false;
                }
                else
                {
                    D100lbl.IsVisible = true;
                    D100lbl.Text = $"100 sided die × {numD100}";
                }
            }

            if (btn.ClassId.ToString() == "D20PlusClass")
            {
                numD20 += 1;
                D20lbl.IsVisible = true;
                D20lbl.Text = $"20 sided die × {numD20}";
            }
            else if (btn.ClassId.ToString() == "D20MinusClass")
            {
                numD20 -= 1;
                if (numD20 <= 0)
                {
                    numD20 = 0;
                    D20lbl.IsVisible = false;
                }
                else
                {
                    D20lbl.IsVisible = true;
                    D20lbl.Text = $"20 sided die × {numD20}";
                }
            }

            if (btn.ClassId.ToString() == "D6PlusClass")
            {
                numD6 += 1;
                D6lbl.IsVisible = true;
                D6lbl.Text = $"6 sided die × {numD6}";
            }
            else if (btn.ClassId.ToString() == "D6MinusClass")
            {
                numD6 -= 1;
                if (numD6 <= 0)
                {
                    numD6 = 0;
                    D6lbl.IsVisible = false;
                }
                else
                {
                    //ResultsD6C.IsVisible = true;
                    D6lbl.Text = $"6 sided die × {numD6}";
                }
            }

            if (btn.ClassId.ToString() == "D4PlusClass")
            {
                numD4 += 1;
                D4lbl.IsVisible = true;
                D4lbl.Text = $"4 sided die × {numD4}";
            }
            else if (btn.ClassId.ToString() == "D4MinusClass")
            {
                numD4 -= 1;
                if (numD4 <= 0)
                {
                    numD4 = 0;
                    D4lbl.IsVisible = false;
                }
                else
                {
                    D4lbl.IsVisible = true;
                    D4lbl.Text = $"4 sided die × {numDx}";
                }
            }

            if (btn.ClassId.ToString() == "DxPlusClass")
            {
                numDx += 1;
                Dxlbl.IsVisible = true;
                Dxlbl.Text = $"Custom die × {numDx}";
            }
            else if (btn.ClassId.ToString() == "DxMinusClass")
            {
                numDx -= 1;
                if (numDx <= 0)
                {
                    numDx = 0;
                    Dxlbl.IsVisible = false;
                }
                else
                {
                    Dxlbl.IsVisible = true;
                    Dxlbl.Text = $"4 sided die × {numDx}";
                }
            }

            if (numD100 > 0 || numD20 > 0 || numD6 > 0 || numD4 > 0 || numDx > 0)
            {
                TurnOnAccelerometer(true);
            }
            else
            {
                TurnOnAccelerometer(false);
            }
        }

        //Turns on accelerometer so dice roll can be triggered by shaking device.
        public void TurnOnAccelerometer(bool flag)
        {
            try
            {
                if (!flag && Accelerometer.IsMonitoring)
                {
                    Accelerometer.Stop();
                    RollButton.IsEnabled = false;
                }

                if (flag && !Accelerometer.IsMonitoring)
                {
                    Accelerometer.Start(speed);
                    RollButton.IsEnabled = true;
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

        // Todo, replace this with a flyout or separate page to enter data, rather than the simple prompt
        public async void SetCustomDiceRange(object sender, EventArgs e)
        {
            CustomDice customDice = new CustomDice();
            await Shell.Current.GoToAsync($"{nameof(CustomDiceConfig)}?{nameof(CustomDiceConfig.DiceId)}={customDice.ID.ToString()}");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (await App.Database.GetDieAsync(1) == null)
            {
                CustomDice setup = new CustomDice();
                setup.ID = 0;
                setup.LowEnd = 1;
                setup.HighEnd = 50;
                await App.Database.SaveDiceAsync(setup);
                Dxrange.Text = $"D50 (1-50)";
            }
            else
            {
                try
                {
                    CustomDice customDice = await App.Database.GetDieAsync(1);
                    BindingContext = customDice;
                    Dxrange.Text = $"D{customDice.HighEnd - (customDice.LowEnd - 1)} ({customDice.LowEnd}-{customDice.HighEnd})";
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to load custom dice.");
                }
                
            }

            // Retrieve all the players from the database, and set them as the
            // data source for the CollectionView.
            //var test = await App.Database.GetDiceAsync();
        }

        public void ShowPopup(string msg)
        {
            App.Current.MainPage.DisplayAlert("Alert:", msg, "Dismiss");
        }

        async void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            // Process shake event        
            RollDice(sender, e);
        }

        public void DetectShake()
        {
            //Register for reading changes, be sure to unsubscribe when finished
            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
        }
    }
}