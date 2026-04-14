using Godot;

public partial class HUD : CanvasLayer
{
	private Label _timerLabel;
	private Label _keyCount;
	private Game game;

	public override async void _Ready()
	{
		_timerLabel = GetNode<Label>("TimerLabel");
        _keyCount = GetNode<Label>("KeyCount");
		game = GetParent<Game>();
    }

    public override void _Process(double delta)
	{
		if (GetTree().Paused)
			return;

		GameManager.Instance.ElapsedTime += (float)delta;
		UpdateTimerDisplay();
	}

	private void UpdateTimerDisplay()
	{
		int totalSeconds = (int)GameManager.Instance.ElapsedTime;
		int minutes = totalSeconds / 60;
		int seconds = totalSeconds % 60;
		_timerLabel.Text = $"{minutes:00}:{seconds:00}";
	}

	public void UpdateKeyCount()
	{
		_keyCount.Text = $"Keys: {game.PlayerKeys}/{game.KeyCount}";
	}
}
