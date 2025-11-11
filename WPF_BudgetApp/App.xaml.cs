using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPF_BudgetApp.Data;
using WPF_BudgetApp.Services;
using WPF_BudgetApp.Services.Interfaces;
using WPF_BudgetApp.ViewModel;

namespace WPF_BudgetApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private IHost? _host;

	protected override async void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		_host = Host.CreateDefaultBuilder()
			.ConfigureServices((context, services) =>
			{
				// DbContext - fichier local dans le dossier de l'app
				services.AddDbContext<AppDbContext>(options =>
					options.UseSqlite("DataSource=BudgetApp.db"));

				// services
				services.AddTransient<IAccountService, AccountService>();
				services.AddTransient<IAppUserService, AppUserService>();
				services.AddTransient<ICategoryService, CategoryService>();
				services.AddTransient<IDebtService, DebtService>();
				services.AddTransient<ITransferService, TransferService>();
				services.AddTransient<IProjectionTransferService, ProjectionTransferService>();
				services.AddTransient<IArchivedTransferService, ArchivedTransferService>();

				// ViewModels
				services.AddSingleton<MainWindow>();
				services.AddTransient<MainViewModel>();
				services.AddTransient<LoginViewModel>();
				services.AddTransient<DashboardViewModel>();
				services.AddTransient<AccountViewModel>();
				services.AddTransient<DebtViewModel>();
				services.AddTransient<ArchiveViewModel>();
			})
			.Build();

		await _host.StartAsync();

		// show main window (DI resolve)
		var main = _host.Services.GetRequiredService<MainWindow>();
		main.Show();
	}
}