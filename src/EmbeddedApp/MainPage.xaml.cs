using System;
using System.Diagnostics;
using Windows.Devices.Gpio;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EmbeddedApp
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const int Ldr0 = 5;
        private const int Ldr1 = 6;
        private readonly Stopwatch _goalTimer;
        private GpioPin _ldr0Pin;
        private GpioPin _ldr1Pin;

        public MainPage()
        {
            InitializeComponent();
            InitGpio();

            _goalTimer = new Stopwatch();
            _goalTimer.Start();
        }

        private void InitGpio()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                _ldr1Pin = null;
                VanityString.Text = "There is no GPIO controller on this device.";
                return;
            }

            _ldr1Pin = gpio.OpenPin(Ldr1);
            _ldr1Pin.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _ldr1Pin.ValueChanged += ldr_ValueChanged;
            _ldr1Pin.DebounceTimeout = TimeSpan.FromMilliseconds(1);

            _ldr0Pin = gpio.OpenPin(Ldr0);
            _ldr0Pin.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _ldr0Pin.ValueChanged += ldr_ValueChanged;
            _ldr0Pin.DebounceTimeout = TimeSpan.FromMilliseconds(1);

            VanityString.Text = "There is no current match on this table.";
        }

        private void ldr_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            // Need to invoke UI updates on the UI thread because this event
            // handler gets invoked on a separate thread.
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (e.Edge == GpioPinEdge.FallingEdge && _goalTimer.ElapsedMilliseconds >= 2000)
                {
                    var goal = new Goal(this);
                    goal.Post(sender.PinNumber == 5 ? Goal.Side.Red : Goal.Side.Yellow);

                    _goalTimer.Restart();
                }
            });
        }

        public void UpdateGui()
        {
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                VanityString.Text = Goal.Results.VanityString;
                if (Goal.Results.Teams != null)
                {
                    BlueTeam.Text = Goal.Results.Teams.BlueTeam.Name ?? "";
                    RedTeam.Text = Goal.Results.Teams.RedTeam.Name ?? "";
                }
                else
                {
                    BlueTeam.Text = "";
                    RedTeam.Text = "";
                }
                if (Goal.Results.Score != null)
                {
                    BlueGoals.Text = Goal.Results.Score.BlueGoals.ToString();
                    RedGoals.Text = Goal.Results.Score.RedGoals.ToString();
                }
                else
                {
                    BlueGoals.Text = "";
                    RedGoals.Text = "";
                }
            });
        }
    }
}