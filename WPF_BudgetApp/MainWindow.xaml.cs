using System.Windows;
using System.Windows.Input;
using WPF_BudgetApp.Services.Interfaces;
using WPF_BudgetApp.ViewModel;

namespace WPF_BudgetApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private bool IsMaximized = false;
	
	public MainWindow(IAccountService accountService, 
	IAppUserService appUserService, 
	ICategoryService categoryService, 
	IDebtService debtService, 
	ITransferService transferService,  
	IArchivedTransferService archiveService)
	{
		InitializeComponent();
		
		MainViewModel vm = new MainViewModel(accountService, appUserService,  categoryService, debtService, transferService, archiveService);
		DataContext = vm;
	}
	
	private void Border_MouseDown(object sender, MouseButtonEventArgs e)
	{
		if(e.ChangedButton == MouseButton.Left)
			DragMove();
	}

	private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		if(e.ClickCount == 2)
			ChangeMaximizeWindow();
	}
	
	public void ChangeMaximizeWindow()
	{
		if (IsMaximized)
		{
			WindowState = WindowState.Normal;
			Width = 1600;
			Height = 900;
			IsMaximized = false;
		} else
		{
			WindowState = WindowState.Maximized;
			IsMaximized = true;
		}
	}
}