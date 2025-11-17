using System.Windows.Controls;
using WPF_BudgetApp.ViewModel;

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

	private void SelectedAccountCB_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if(DataContext != null)
			((AccountViewModel)DataContext).UpdateSelectedAccount();
	}
}