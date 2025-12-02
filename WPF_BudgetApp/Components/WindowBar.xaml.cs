using System.Windows;
using System.Windows.Controls;

namespace WPF_BudgetApp.Components;

public partial class WindowBar : UserControl
{
	public WindowBar()
	{
		InitializeComponent();
	}
	
	private void Close_But_OnClick(object sender, RoutedEventArgs e)
	{
		MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
		if (mainWindow == null) return;
		
		mainWindow.Close();
	}

	private void Max_But_OnClick(object sender, RoutedEventArgs e)
	{
		MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
		if (mainWindow == null) return;
		
		mainWindow.ChangeMaximizeWindow();
	}

	private void Min_But_OnClick(object sender, RoutedEventArgs e)
	{
		MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
		if (mainWindow == null) return;
		
		mainWindow.WindowState = WindowState.Minimized;
	}
}