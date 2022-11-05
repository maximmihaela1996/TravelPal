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
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{

    public partial class TravelsWindow : Window
    {
        private UserManager userManager;
        private TravelManager travelManager;
        private User signedInUser;
        private bool isAdmin;
        public TravelsWindow(UserManager userManager, TravelManager travelManager, bool isAdmin)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.travelManager = travelManager;
            signedInUser = userManager.SignedInUser as User;
            this.isAdmin = isAdmin;
            lblUsername.Content = $"Hello dear: {userManager.SignedInUser.Username}!";
            UpdateTravelsList();
            UpdateAdminUI();   
        }

        // Updates the UI based on user type (user or admin)
        private void UpdateAdminUI()
        {
            if (isAdmin)
            {
                EnableButtons();
            }
        }

        private void EnableButtons()
        {
            btnAddTravel.IsEnabled = false;
            btnTravelDetails.IsEnabled = false;
            btnInfo.IsEnabled = false;
            btnUserDetails.IsEnabled = false;
        }

        // Updates travels list based on user type (user or admin)
        private void UpdateTravelsList()
        {
            if (isAdmin && travelManager.Travels.Count() != 0)
            {
                foreach (Travel travel in travelManager.Travels)
                {
                    ListViewItem listViewItem = new();
                    listViewItem.Tag = travel;
                    listViewItem.Content = travel.GetInfo();
                    lvTravels.Items.Add(listViewItem);
                }
            }
            else if (!isAdmin && signedInUser.Travels.Count() != 0)
            {
                foreach (Travel travel in signedInUser.Travels)
                {
                    ListViewItem listViewItem = new();
                    listViewItem.Tag = travel;
                    listViewItem.Content = travel.GetInfo();
                    lvTravels.Items.Add(listViewItem);
                }
            }
        }

        // Creates new instance of MainWindow, passing all necessary information and closes this window
        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new(userManager, travelManager);
            mainWindow.Show();
            this.Close();
        }

        // Creates a new instance of TravelDetailsWindow displaying details about selected travel, passing all necessary information. In case of no selection, displays MessageBox to user informing what is wrong
        private void btnTravelDetails_Click(object sender, RoutedEventArgs e)
        {
            TravelDetailsWindow travelDetailsWindow;

            if (lvTravels.SelectedItem != null)
            {
                ListViewItem selectedItem = lvTravels.SelectedItem as ListViewItem;
                travelDetailsWindow = new(selectedItem.Tag as Travel, travelManager);
                travelDetailsWindow.ShowDialog();

            }
            else
            {
                MessageBox.Show("Selection required!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Event that triggers every time window gets activated, making sure travel list is updated after any possible changes
        private void Window_Activated(object sender, EventArgs e)
        {
            lvTravels.Items.Clear();
            UpdateTravelsList();
        }
        // Creates a new instance of UserDetailsWindow, passing necessary information
        private void btnUserDetails_Click(object sender, RoutedEventArgs e)
        {
            UserDetailsWindow userDetailsWindow = new(userManager, this);
            userDetailsWindow.ShowDialog();
        }

        // Displays MessageBox with some info
        private void btnInfo_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("with the travelPal application you can plan your trips efficiently. Save time and energy! " +
               " Warning! Select first from the list if you want to delete a trip!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        //  Remove action Based on user type
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            //Verify if users has selected an row
            if (lvTravels.SelectedItem != null)
            {
                //Check user type and remove it
                if (isAdmin)
                {
                    ListViewItem selectedItem = lvTravels.SelectedItem as ListViewItem;
                    travelManager.AdminRemoveTravel(selectedItem.Tag as Travel);
                    lvTravels.Items.RemoveAt(lvTravels.SelectedIndex);
                }
                else
                {
                    ListViewItem selectedItem = lvTravels.SelectedItem as ListViewItem;
                    travelManager.RemoveTravel(selectedItem.Tag as Travel);
                    lvTravels.Items.RemoveAt(lvTravels.SelectedIndex);
                }
            }
            else
            {
                MessageBox.Show("You must select an row first!", "warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Creates a new instance of AddTravelWindow, passing all necessary information
        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new(userManager, travelManager);
            addTravelWindow.ShowDialog();
        }
        // Back to the MainWindow
        private void btnSignOut_Click_2(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new(userManager, travelManager);
            mainWindow.Show();
            this.Close();
        }
        private void lvTravels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
