using System.Windows.Controls;

namespace WPF_BudgetApp.Views;

public partial class DebtView : UserControl
{
	public DebtView()
	{
		InitializeComponent();
	}

	private void DebtGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		var grid = (DataGrid)sender;
		updateDebtButt.IsEnabled = grid.SelectedItems.Count == 1;
		deleteDebtButt.IsEnabled = grid.SelectedItems.Count == 1;
	}
}