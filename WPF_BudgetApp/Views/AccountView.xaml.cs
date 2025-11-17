using System.Windows.Controls;

namespace WPF_BudgetApp.Views;

public partial class AccountView : UserControl
{
	public AccountView()
	{
		InitializeComponent();
	}

	private void TransferGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		var grid = (DataGrid)sender;
		updateTransferButt.IsEnabled = grid.SelectedItems.Count == 1;
		deleteTransferButt.IsEnabled = grid.SelectedItems.Count == 1;
	}
}