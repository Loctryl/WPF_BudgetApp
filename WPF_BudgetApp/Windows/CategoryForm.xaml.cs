using System.Windows;
using System.Windows.Input;
using WPF_BudgetApp.ViewModel;

namespace WPF_BudgetApp.Windows;

public partial class CategoryForm : Window
{
	public event EventHandler ConfirmEvent;

	public bool IsUpdate;
	
	public CategoryForm(DashboardViewModel parentVM, bool isUpdate)
	{
		InitializeComponent();
		DataContext = parentVM.CatFormDTO;
		IsUpdate = isUpdate;
	}
	
	private void Border_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if(e.ChangedButton == MouseButton.Left)
			DragMove();
	}

	private void ConfirmButt_OnClick(object sender, RoutedEventArgs e)
	{
		ConfirmEvent?.Invoke(sender, e);
		Close();
	}

	private void CancelButt_OnClick(object sender, RoutedEventArgs e)
	{
		Close();
	}
}