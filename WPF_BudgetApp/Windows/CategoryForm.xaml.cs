using System.Windows;
using System.Windows.Input;
using WPF_BudgetApp.ViewModel;

namespace WPF_BudgetApp.Windows;

public partial class CategoryForm : Window
{
	public CategoryForm(DashboardViewModel parentVM)
	{
		InitializeComponent();
		DataContext = parentVM;
	}
	
	private void Border_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if(e.ChangedButton == MouseButton.Left)
			DragMove();
	}

	private void ConfirmButt_OnClick(object sender, RoutedEventArgs e)
	{
		
	}

	private void CancelButt_OnClick(object sender, RoutedEventArgs e)
	{
		Close();
	}
}