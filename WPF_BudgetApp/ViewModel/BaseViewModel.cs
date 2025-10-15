using System.ComponentModel;
using PropertyChanged;

namespace WPF_BudgetApp.ViewModel;

[AddINotifyPropertyChangedInterface]
public class BaseViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => {};
}