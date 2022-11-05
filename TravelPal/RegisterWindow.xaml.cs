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
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    public partial class RegisterWindow : Window
    {
        private UserManager userManager;
        public RegisterWindow(UserManager userManager)
        {
            InitializeComponent();
            this.userManager = userManager;
            cbCountries.ItemsSource = Enum.GetValues(typeof(Countries));
        }

      
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            //Check if method ValidateInput return true, if returns meens that all the inputs is in a correct form
            if (ValidateInput())
            {
                //Create a new user with specific values
                User newUser = new(txtUsername.Text, txtPassword.Password, (Countries)cbCountries.SelectedItem);
                //if user added succesfully, then an messageBox will appear
                if (userManager.AddUser(newUser))
                {
                    MessageBox.Show("Registration successful!", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                //else if don't meens that user is already taked
                    MessageBox.Show("User already exists!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        // Checks if all the fields are filled
        private bool ValidateInput()
        {
            if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Password) && !string.IsNullOrEmpty(txtConfirmPassword.Password) && cbCountries.SelectedItem != null)
            {
                //booleans that return if the method ValidateUsernameLength from userManager return true eller false
                bool isValidUsernameLength = userManager.ValidateUsernameLength(txtUsername.Text);
                bool isValidPasswordLength = userManager.ValidatePasswordLength(txtConfirmPassword.Password);
                //check usersLength is correct
                if (isValidUsernameLength)
                {
                    if (isValidPasswordLength)
                    {
                        //chech if passwords match each other
                        if (txtPassword.Password == txtConfirmPassword.Password)
                        {
                            return true;
                        }
                        else
                        {
                            //ErrorMessage
                            MessageBox.Show("Passwords must match!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password must be at least 5 characters!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Username must be at least 3 characters!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("All fields must be filled!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }
    }
}
