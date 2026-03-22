using Godot;

public partial class HUD : CanvasLayer
{
	private Label _timerLabel;

	public override void _Ready()
	{
		_timerLabel = GetNode<Label>("TimerLabel");
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
}
