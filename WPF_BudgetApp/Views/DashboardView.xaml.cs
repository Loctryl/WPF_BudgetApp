using System.Windows.Controls;

namespace WPF_BudgetApp.Views;

public partial class DashboardView : UserControl
{
	public DashboardView()
	{
		InitializeComponent();
	}

	private void CategoryGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		var grid = (DataGrid)sender;
		updateCategoryButt.IsEnabled = grid.SelectedItems.Count == 1;
		deleteCategoryButt.IsEnabled = grid.SelectedItems.Count == 1;
	}
}