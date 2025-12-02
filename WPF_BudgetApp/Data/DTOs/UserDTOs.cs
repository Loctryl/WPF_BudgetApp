namespace WPF_BudgetApp.Data.DTOs;

public class UserFormDTO
{
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string PrimaryColor { get; set; } = string.Empty;
	public string SecondaryColor { get; set; } = string.Empty;
	public string TertiaryColor { get; set; } = string.Empty;
	public string WritingColor { get; set; } = string.Empty;

	public void Reset()
	{
		Username = string.Empty;
		Password = string.Empty;
		PrimaryColor = string.Empty;
		SecondaryColor = string.Empty;
		TertiaryColor = string.Empty;
		WritingColor = string.Empty;
	}
}