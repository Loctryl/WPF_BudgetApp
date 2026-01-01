using System.Windows.Input;
using WPF_BudgetApp.Commands;
using WPF_BudgetApp.Data.DTOs;
using WPF_BudgetApp.Data.Models;
using WPF_BudgetApp.Resources;

namespace WPF_BudgetApp.ViewModel;

public class ArchiveViewModel : BaseMenuViewModel
{
	public List<ArchivedTransfer> ArchivedTransfers { get; set; } = new List<ArchivedTransfer>();
	public ArchiveQuery Query { get; set; } = new ArchiveQuery();
	
	public ICommand SearchCommand { get; } 
	
	
	public ArchiveViewModel(MainViewModel mainVM) : base(mainVM)
	{
		SearchCommand = new RelayCommand(_ => UpdateArchives());
	}
	
	#region Updating Data
	
	public override void UpdateData()
	{
		base.UpdateData();
	}

	private void UpdateArchives()
	{
		ArchivedTransfers.Clear();
		ArchivedTransfers.AddRange(mainVM.archiveService.GetAllArchivedTransfersWithQueryAsync(mainVM.CurrentUser.Id, Query).Result);
	}
	
	#endregion
}