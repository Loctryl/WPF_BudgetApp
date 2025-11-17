using System.Windows;
using System.Windows.Controls;
using WPF_BudgetApp.ViewModel;

namespace WPF_BudgetApp.Views;

public partial class LoginView : UserControl
{
	private bool IsUsernameValid;
	private bool IsPasswordValid;
	public LoginView()
	{
		InitializeComponent();
	}
	
	private void SwitchButton_OnClick(object sender, RoutedEventArgs e)
	{
		logInUsername.Clear();
		logInPassword.Clear();
		registerUsername.Clear();
		registerPassword.Clear();
		consentCheckBox.IsChecked = false;
		registerButton.IsEnabled = false;
		logInButton.IsEnabled = false;
		IsUsernameValid = false;
		IsPasswordValid = false;

		if (LoginGrid.Visibility == Visibility.Visible)
		{
			LoginGrid.Visibility = Visibility.Collapsed;
			RegisterGrid.Visibility = Visibility.Visible;
		}
		else
		{
			RegisterGrid.Visibility = Visibility.Collapsed;
			LoginGrid.Visibility = Visibility.Visible;
		}
	}

	private void TextBox_OnChange(object sender, RoutedEventArgs e)
	{
		var textBox = (TextBox)sender;
		IsUsernameValid = textBox.Text != string.Empty;
		CheckButtonAvailability(sender, e);
	}
	
	private void PasswordBox_OnChange(object sender, RoutedEventArgs e)
	{
		if (this.DataContext != null)
		{ ((LoginViewModel)this.DataContext).Password = ((PasswordBox)sender).Password; }
		
		var passwordBox = (PasswordBox)sender;
		IsPasswordValid = passwordBox.Password != string.Empty;
		CheckButtonAvailability(sender, e);
	}

	private void CheckButtonAvailability(object sender, RoutedEventArgs e)
	{
		if (!IsUsernameValid || !IsPasswordValid)
		{
			registerButton.IsEnabled = logInButton.IsEnabled = false;
			return;
		}
		
		if(!consentCheckBox.IsVisible)
			logInButton.IsEnabled = true;
		else if(consentCheckBox.IsChecked == true)
			registerButton.IsEnabled = true;
		else
			registerButton.IsEnabled = false;
	}
}