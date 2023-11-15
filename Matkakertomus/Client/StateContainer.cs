using Matkakertomus.Shared;

public class StateContainer
{
	private ProfileDto? user;
	public ProfileDto User
	{
		get => user;
		set
		{
			user = value;
			NotifyStateChanged();
		}
	}

	public event Action? OnChange;

	private void NotifyStateChanged() => OnChange?.Invoke();
}