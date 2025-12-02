using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_BudgetApp.Data.DTOs;

namespace WPF_BudgetApp.Windows;

public partial class UserForm : Window
{
	public event EventHandler<bool>? ConfirmEvent;
	
	public UserForm(UserFormDTO dataContext)
	{
		InitializeComponent();
		Topmost = true;
		Deactivated += Window_Deactivated;
		
		DataContext = dataContext;
		userPassword.Password = dataContext.Password;
	}
	
	private void PasswordBox_OnChange(object sender, RoutedEventArgs e)
	{
		if (DataContext != null)
		{ ((UserFormDTO)DataContext).Password = ((PasswordBox)sender).Password; }
	}

	private void EyeButt_Activate(object sender, RoutedEventArgs e)
	{
		passwordTB.Text = userPassword.Password;
		userPassword.Visibility = Visibility.Collapsed;
		passwordTB.Visibility = Visibility.Visible;
	}
	
	private void EyeButt_Deactivate(object sender, RoutedEventArgs e)
	{
		passwordTB.Text = string.Empty;
		userPassword.Visibility = Visibility.Visible;
		passwordTB.Visibility = Visibility.Collapsed;
	}
	
	#region Window Events
	
	private void Window_Deactivated(object? sender, EventArgs e)
	{
		Activate();
		Focus();
	}
	
	private void Border_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if(e.ChangedButton == MouseButton.Left)
			DragMove();
	}
	
	private void ConfirmButt_OnClick(object sender, RoutedEventArgs e)
	{
		ConfirmEvent?.Invoke(this, true);
		UnsubscribeAllEvents();
		Close();
	}

	private void CancelButt_OnClick(object sender, RoutedEventArgs e)
	{
		ConfirmEvent?.Invoke(this, false);
		UnsubscribeAllEvents();
		Close();
	}

	private void UnsubscribeAllEvents()
	{
		if (ConfirmEvent == null) return;
		foreach (var d in ConfirmEvent.GetInvocationList())
			ConfirmEvent -= (d as EventHandler<bool>);
		Deactivated -= Window_Deactivated;
	}
	
	#endregion
}