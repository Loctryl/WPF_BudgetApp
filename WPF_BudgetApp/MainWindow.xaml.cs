using System.Windows;
using System.Windows.Input;

namespace WPF_BudgetApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private bool IsMaximized = false;
	
	public MainWindow()
	{
		InitializeComponent();
	}
	
	private void Border_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if(e.ChangedButton == MouseButton.Left)
			DragMove();
	}

	private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if(e.ClickCount == 2)
			MaximizeWindow();
	}
	
	public void MaximizeWindow()
	{
		if (IsMaximized)
		{
			WindowState = WindowState.Normal;
			Width = 1080;
			Height = 720;
			IsMaximized = false;
		} else
		{
			WindowState = WindowState.Maximized;
			IsMaximized = true;
		}
	}
}