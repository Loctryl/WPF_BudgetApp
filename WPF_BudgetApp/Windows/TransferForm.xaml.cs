using System.Windows;
using System.Windows.Input;
using WPF_BudgetApp.ViewModel;

namespace WPF_BudgetApp.Windows;

public partial class TransferForm : Window
{
	public event EventHandler<bool>? ConfirmEvent;
	public readonly bool IsUpdate;
	
	public TransferForm(AccountViewModel parentVM, bool isUpdate)
	{
		InitializeComponent();
		Topmost = true;
		Deactivated += Window_Deactivated;
		
		DataContext = parentVM.TransFormDTO;
		IsUpdate = isUpdate;
	}
	
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
}