using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelPal.Enums;
using TravelPal.Interfaces;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    public partial class AddTravelWindow : Window
    {
        private TravelManager travelManager;
        private User? signedInUser;
        public AddTravelWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();
            this.travelManager = travelManager;
            this.signedInUser = userManager.SignedInUser as User;

            cbDetailsCountry.ItemsSource = Enum.GetValues(typeof(Countries));
            cbTravelType.ItemsSource = travelManager.TravelTypes;
            cbTripType.ItemsSource = Enum.GetValues(typeof(TripTypes));

            dpStartingDate.DisplayDateStart = DateTime.Now;
            dpEndingDate.DisplayDateStart = DateTime.Now;
        }

        // Determines the travel type (Trip or Vacation)
        private string DetermineTravelType()
        {
            if (cbTravelType.SelectedItem != null)
            {
                if (cbTravelType.SelectedItem == "Trip")
                {
                    return "Trip";
                }
                else if (cbTravelType.SelectedItem == "Vacation")
                {
                    return "Vacation";
                }
            }
            return null;
        }

        // Modifies the UI when Trip or Vacation has been selected
        private void ModifyTravelType(string travelType)
        {
            if (travelType == "Trip")
            {
                xbAllInclusive.Visibility = Visibility.Hidden;
                txtTripType.Visibility = Visibility.Visible;
                cbTripType.Visibility = Visibility.Visible;
                cbTripType.ItemsSource = Enum.GetValues(typeof(TripTypes));
            }
            else if (travelType == "Vacation")
            {
                txtTripType.Visibility = Visibility.Hidden;
                cbTripType.Visibility = Visibility.Hidden;
                xbAllInclusive.Visibility = Visibility.Visible;
            }
        }

        // Checks if all fields are filled correctly
        private bool ValidateInput()
        {
            if (dpStartingDate.SelectedDate != null && dpEndingDate.SelectedDate != null && !string.IsNullOrEmpty(tbDestination.Text) && !string.IsNullOrEmpty(tbTravelers.Text) && cbTravelType.SelectedItem != null && cbDetailsCountry.SelectedItem != null)
            {
                if (dpEndingDate.SelectedDate >= dpStartingDate.SelectedDate)
                {
                    if (int.TryParse(tbTravelers.Text, out int result))
                    {
                        if (DetermineTravelType() == "Trip")
                        {
                            if (cbTripType.SelectedItem != null)
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Please choose a trip type!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return false;
                            }
                        }
                        else if (DetermineTravelType() == "Vacation")
                        {
                            return true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please input a valid amount of travellers!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("How you gon' travel back in time? Ending date has to be later than starting date yo", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("All fields must be filled!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return false;
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                if (DetermineTravelType() == "Trip")
                {
                    Trip newTrip = new(tbDestination.Text, (Countries)cbDetailsCountry.SelectedItem, Convert.ToInt32(tbTravelers.Text), (DateTime)dpStartingDate.SelectedDate, (DateTime)dpEndingDate.SelectedDate, (TripTypes)cbTripType.SelectedItem, signedInUser);
                    travelManager.AddTravel(newTrip);
                }
                else if (DetermineTravelType() == "Vacation")
                {
                    Vacation newVacation;

                    if (xbAllInclusive.IsChecked == true)
                    {
                        newVacation = new(tbDestination.Text, (Countries)cbDetailsCountry.SelectedItem, Convert.ToInt32(tbTravelers.Text), (DateTime)dpStartingDate.SelectedDate, (DateTime)dpEndingDate.SelectedDate, true, signedInUser);

                    }
                    else
                    {
                        newVacation = new(tbDestination.Text, (Countries)cbDetailsCountry.SelectedItem, Convert.ToInt32(tbTravelers.Text), (DateTime)dpStartingDate.SelectedDate, (DateTime)dpEndingDate.SelectedDate, false, signedInUser);
                    }
                    travelManager.AddTravel(newVacation);
                }

                MessageBox.Show("Added!");
                this.Close();
            }
        }

        private void cbTravelType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModifyTravelType(DetermineTravelType());
        }

        private void cbDetailsCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
