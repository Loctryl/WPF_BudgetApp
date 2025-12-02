using System.Windows;
using System.Windows.Input;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Resources;
using WPF_BudgetApp.ViewModel;

namespace WPF_BudgetApp.Windows;

public partial class DebtForm : Window
{
	public event EventHandler<bool>? ConfirmEvent;
	public readonly FormType FormType;
	
	public DebtForm(DebtFormDTO dataContext, FormType formType)
	{
		InitializeComponent();
		Topmost = true;
		Deactivated += Window_Deactivated;
		
		DataContext =  dataContext;
		FormType = formType;
		
		switch (FormType)
		{
			case FormType.ADD:
				mainGrid.ColumnDefinitions[0].Width = new GridLength(25, GridUnitType.Star);
				mainGrid.ColumnDefinitions[1].Width = new GridLength(50, GridUnitType.Star);
				mainGrid.ColumnDefinitions[2].Width = new GridLength(25, GridUnitType.Star);
				addFields.Visibility = Visibility.Visible;
				break;
			case FormType.EDIT:
				editFields.Visibility = Visibility.Visible;
				break;
			case FormType.DELETE:
				deleteFields.Visibility = Visibility.Visible;
				ConfirmButt.IsEnabled = true;
				break;
		}
	}
	
	private void AddConfirmAvailability(object sender, RoutedEventArgs e)
	{
		if(FormType is FormType.DELETE or FormType.EDIT) return;
		
		if (!string.IsNullOrEmpty(addDebtName.Text) 
		    && (addDebtInitAmount.Text != "0" && !string.IsNullOrEmpty(addDebtInitAmount.Text))
		    && (addDebtCurrAmount.Text != "0" && !string.IsNullOrEmpty(addDebtCurrAmount.Text)) 
		    && addDebtAccount.SelectedItem != null)
		{
			ConfirmButt.IsEnabled = true;
		}
		else
			ConfirmButt.IsEnabled = false;
		
	}
	
	private void EditConfirmAvailability(object sender, RoutedEventArgs e)
	{
		if(FormType is FormType.DELETE or FormType.ADD) return;
		
		if (!string.IsNullOrEmpty(editDebtName.Text) 
		    && (editDebtCurrAmount.Text != "0" && !string.IsNullOrEmpty(editDebtCurrAmount.Text)))
		{
			ConfirmButt.IsEnabled = true;
		}
		else
			ConfirmButt.IsEnabled = false;
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