using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelPal.Enums;
using TravelPal.Interfaces;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public class UserManager
    {
        public List<IUser> Users { get; set; } = new();
        public IUser SignedInUser { get; set; }

        public UserManager()
        {
            CreateDefaultUsers();
        }

        // Populate User and travel list with default user and travel 
        private void CreateDefaultUsers()
        {
            Admin admin = new("admin", "password", Countries.Japan);
            Users.Add(admin);

            User gandalf = new("Gandalf", "password", Countries.Korea);
            Users.Add(gandalf);
            Vacation defaultVacation = new("Paris", Countries.France, 2, new DateTime(2022, 10, 10), new DateTime(2022, 10, 15), true, gandalf);

            gandalf.Travels.Add(defaultVacation);
            Trip defaultTrip = new("Milano", Countries.Italy, 1, new DateTime(2022, 11, 15), new DateTime(2023, 01, 05), TripTypes.Work, gandalf);

            gandalf.Travels.Add(defaultTrip);
        }

        // Adds user to the users list if validation is successful
        public bool AddUser(IUser newUser)
        {
            if (ValidateUsername(newUser.Username))
            {
                Users.Add(newUser);
                return true;
            }
            return false;
        }

        // Updates username, password and location of user that is currently signed in
        public bool UpdateUser(IUser userToUpdate)
        {
            if (ValidateUsername(userToUpdate.Username) || userToUpdate.Username == SignedInUser.Username)
            {
                SignedInUser.Username = userToUpdate.Username;
                SignedInUser.Password = userToUpdate.Password;
                SignedInUser.Location = userToUpdate.Location;
                return true;
            }
            return false;
        }
        public bool UpdateUsername(IUser user, string password)
        {
            return false;
        }
        
        // Checks if provided username length is at least 3 characters long
        public bool ValidateUsernameLength(string username)
        {
            if (username.Length < 3)
            {
                return false;
            }
            return true;
        }

        // Checks if provided password length is at least 5 characters long
        public bool ValidatePasswordLength(string password)
        {
            if (password.Length < 5)
            {
                return false;
            }
            return true;
        }

        // Checks if provided username already exists in the users list, if not = validation successful and username can be used
        private bool ValidateUsername(string username)
        {
            foreach (IUser user in Users)
            {
                if (user.Username == username)
                {
                    return false;
                }
            }
            return true;
        }

        // Checks whether user with provided username and password exists in the users list. If true, sets SignedInUSer to that user and returns true.
        public bool SignInUser(string username, string password)
        {

            foreach (IUser user in Users)
            {
                if (user.Username == username && user.Password == password)
                {
                    SignedInUser = user;
                    return true;
                }
            }
            return false;
        }

        // Check if currently signed in user is an admin
        public bool CheckIfAdmin()
        {
            if (SignedInUser is Admin)
            {
                return true;
            }
            return false;
        }
    }
}
