﻿using PWManager.Accounts;
using PWManager.Models;
using PWManager.Services;
/*
||  Copyright 2014 Daniel Hamacher
|| 
||  Licensed under the Apache License, Version 2.0 (the "License");
||  you may not use this file except in compliance with the License.
||  You may obtain a copy of the License at
||
||      http://www.apache.org/licenses/LICENSE-2.0
||
||  Unless required by applicable law or agreed to in writing, software
||  distributed under the License is distributed on an "AS IS" BASIS,
||  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
||  See the License for the specific language governing permissions and
||  limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PWManager.Users
{
    public class UserViewModel : BindableBase
    {
        
        private IUserRepository _repo = new UserRepository();
        public ICommand Login { get; set; }
        public ICommand Register { get; set; }
        public ICommand Save { get; set; }
        public ICommand Back { get; set; }

        public UserViewModel()
        {
            Login = new RelayCommand<object>(UserLogin);
            Register = new RelayCommand(UserRegistration);
            Save = new RelayCommand<object>(AddOrUpdateUser);
            Back = new RelayCommand(NavigateBack);
        }

        #region Properties
        private User _currentUser = new User();
        public User CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                if (_currentUser == value) { return; }
                _currentUser = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentUser"));
            }
        }

        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (_username == value) { return; }
                _username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                if (_email == value) { return; }
                _email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        private string _firstname;
        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (_firstname == value) { return; }
                _firstname = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Firstname"));
            }
        }

        private string _lastname;
        public string Lastname
        {
            get { return _lastname; }
            set
            {
                if (_lastname == value) { return; }
                _lastname = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Lastname"));
            }
        }
        #endregion


        #region Commands
        /// <summary>
        /// 
        /// </summary>
        private void UserLogin(object pwBox)
        {
            PasswordBox pw = pwBox as PasswordBox;
            try
            {
                CurrentUser = _repo.GetValidatedUserAsync(CurrentUser.Username, pw.Password).Result;
                if (CurrentUser != null)
                {                    
                    Navigator.Navigate(new AccountView(CurrentUser));
                }
            }
            catch (Exception)
            {
                MessageDialog.PromptError("User does not exist or username or password are not valid");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UserRegistration()
        {
            Navigator.Navigate(new UserView(this));
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddOrUpdateUser(object pwBox)
        {
            PasswordBox pw = pwBox as PasswordBox;
            if (CurrentUser.Id == Guid.Empty)
            {
                CurrentUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = this.Username,
                    Password = Security.Security.HashPassword(pw.Password),
                    Firstname = this.Firstname,
                    Lastname = this.Lastname,
                    Email = this.Email
                };
                _repo.AddUserAsync(CurrentUser);
                Navigator.Navigate(new Login());
            }
            else
            {
                CurrentUser.Password = Security.Security.HashPassword(pw.Password);
                _repo.UpdateUserAsync(CurrentUser);
                Navigator.Navigate(new AccountView(CurrentUser));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void NavigateBack()
        {
            if (CurrentUser.Id == Guid.Empty)
            {
                Navigator.Navigate(new Login());
            }
            else
            {
                Navigator.Navigate(new AccountView(CurrentUser));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
