using System.Windows;

namespace WPF_BudgetApp.Windows;

public partial class MessageBox : Window
{
	public MessageBox(string message)
	{
		InitializeComponent();
		Topmost = true;
		Deactivated += Window_Deactivated;
		
		textDisplay.Text = message;
	}
	
	private void Window_Deactivated(object? sender, EventArgs e)
	{
		Activate();
		Focus();
	}
	
	private void ConfirmButt_OnClick(object sender, RoutedEventArgs e) => Close();
}