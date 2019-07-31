using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ResistorUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Calculate_Button_Click(object sender, RoutedEventArgs e)
        {
            String temp = DesiredRatio.Text;
            int tempInt = Percentage.SelectedIndex;
            int percentage;
            float ratio;
            try
            {
                ratio = float.Parse(temp, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch(FormatException fe)
            {
                ErrorDialogBoxPopup(fe.Message);
                DesiredRatio.Focus(FocusState.Programmatic);
                return;
            }

            ResistorOptimizer resistorDividerEngine = new ResistorOptimizer();
            ResistorDividerValues calculatedValues;
            switch(tempInt)
            {
                case 0:
                    percentage = 10;
                    break;
                case 1:
                    percentage = 5;
                    break;
                case 2:
                    percentage = 2;
                    break;
                case 3:
                    percentage = 1;
                    break;
                default:
                    percentage = 10;
                    break;
            }

            calculatedValues = resistorDividerEngine.bruteForce(percentage, ratio);
            TopResistor.Text = calculatedValues.R1.ToString();
            SideResistor.Text = calculatedValues.R2.ToString();
            ErrorBand.Text = calculatedValues.error.ToString("E08");
            MaximumValue.Text = calculatedValues.worstCaseError1.ToString("F04");
            MinimumValue.Text = calculatedValues.worstCaseError2.ToString("F04");
        }

        private async void ErrorDialogBoxPopup(String msg)
        {
            MessageDialog showDialog = new MessageDialog(msg);
            showDialog.Commands.Add(new UICommand("OK")
            {
                Id = 0
            });
            showDialog.DefaultCommandIndex = 0;
            await showDialog.ShowAsync();
        }

        private void DesiredRatio_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Percentage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
