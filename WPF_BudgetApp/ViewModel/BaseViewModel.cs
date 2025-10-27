using System.ComponentModel;
using PropertyChanged;

namespace WPF_BudgetApp.ViewModel;

[AddINotifyPropertyChangedInterface]
public abstract class BaseViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => {};
}