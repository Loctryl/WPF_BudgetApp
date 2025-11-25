using System.Windows;
using System.Windows.Input;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Resources;
using WPF_BudgetApp.ViewModel;

namespace WPF_BudgetApp.Windows;

public partial class TransferForm : Window
{
	public event EventHandler<bool>? ConfirmEvent;
	public readonly FormType FormType;
	
	public TransferForm(TransferFormDTO dataContext, FormType formType)
	{
		InitializeComponent();
		Topmost = true;
		Deactivated += Window_Deactivated;
		
		DataContext = dataContext;
		FormType = formType;
		
		switch (FormType)
		{
			case FormType.ADD:
			case FormType.EDIT:
				addEditFields.Visibility = Visibility.Visible;
				break;
			case FormType.DELETE:
				deleteFields.Visibility = Visibility.Visible;
				ConfirmButt.IsEnabled = true;
				break;
		}
	}
	
	private void AddEditConfirmAvailability(object sender, RoutedEventArgs e)
	{
		if (!string.IsNullOrEmpty(transferNameTB.Text) 
		    && (transferAmountTB.Text != "0" && !string.IsNullOrEmpty(transferNameTB.Text)) 
		    && transferCategoryCB.SelectedItem != null)
		{
			ConfirmButt.IsEnabled = true;
		}
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