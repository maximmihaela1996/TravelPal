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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    public partial class MainWindow : Window
    {
        //Create instans of userManager and travelManager class
        private UserManager userManager;
        private TravelManager travelManager;
        public MainWindow()
        {
            InitializeComponent();
            this.userManager = new();
        }
        public MainWindow(UserManager userManager, TravelManager travelManager)
        {
            InitializeComponent();
            this.userManager = userManager;
            this.travelManager = travelManager;
        }

        // RegisterWindow opens at the button click event/ click on the button
        private void btnRegister_Click_1(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new(userManager);
            registerWindow.ShowDialog();
        }
        // Checks if user is admin or user, and creates new instance of travelswindow
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            bool isUserFound = userManager.SignInUser(txtUsername.Text, txtPassword.Password);
            bool isAdmin = userManager.CheckIfAdmin();
            //if an user is found then an travelsWindow instance is created and then TravelsWindow window opens
            if (isUserFound)
            {
                // Clear inputs
                txtUsername.Clear();
                txtPassword.Clear();

                TravelsWindow travelsWindow = new(userManager, travelManager, isAdmin);
                travelsWindow.Show();
                this.Close();
            }
            //errorMessageBox shows
            else
            {
                MessageBox.Show("The username or password doesn't match", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
