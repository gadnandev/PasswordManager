﻿using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.ObjectModel;
using PWManager.ViewModels;

namespace PWManager
{
	/// <summary>
	/// Interaction logic for AccountScreen.xaml
	/// </summary>
	public partial class AccountScreen : UserControl
	{
        private Guid userId;
        private ObservableCollection<AccountViewModel> accountList = new ObservableCollection<AccountViewModel>();
        

		public AccountScreen(Guid id)
		{
            userId = id;
            accountList = UserViewModel.GetUserAccounts(userId);
            this.DataContext = accountList;
			this.InitializeComponent();
		}

        private void SignOUtBtn_Click(object sender, RoutedEventArgs e)
        {
            Navigator.Navigate(new LoginScreen());
        }       

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item.Header.Equals("New Account"))
            {
                Navigator.Navigate(new NewAccountScreen(userId));
            }

        }

        private void UpdateBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Navigator.Navigate(new UserScreen(userId));
        }

        private void AccountListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox items = sender as ListBox;
                AccountViewModel acc = AccountViewModel.GetAccountByName(userId, AccountListBox.SelectedItem.ToString());
                AccountNameTextBox.Text = acc.Name;
                AccountPasswordTextBox.Text = Security.Security.EncryptPassword(acc.LoginPassword);
                AccountUsernameTextBox.Text = acc.LoginName;
                AccountCommentsTextBox.Text = acc.Comments;
                AccountLinkTextBox.Text = acc.Link;
            }
            catch (Exception ex)
            {
                PromptError(ex.Message);
            }

        }

        private void PromptError(string msg)
        {
            const string caption = "Error";
            MessageBoxImage icon = MessageBoxImage.Error;
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBox.Show(msg, caption, button, icon);
        }

        private void PromptInfo(string msg)
        {
            const string caption = "Info";
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBox.Show(msg, caption, button, icon);
        }              
	}
}