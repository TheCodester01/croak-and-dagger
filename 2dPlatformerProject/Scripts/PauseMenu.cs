using Godot;

public partial class PauseMenu : CanvasLayer
{
	public override void _Ready()
	{
		Hide();
		ProcessMode = ProcessModeEnum.Always;

		GetNode<Button>("Panel/VBox/ContinueButton").Pressed += OnContinue;
		GetNode<Button>("Panel/VBox/MainMenuButton").Pressed += OnMainMenu;
		GetNode<Button>("Panel/VBox/QuitButton").Pressed += OnQuit;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			if (GetTree().Paused)
				OnContinue();
			else
				OpenPauseMenu();
		}
	}

	private void OpenPauseMenu()
	{
		GetTree().Paused = true;
		Show();
	}

	private void OnContinue()
	{
		GetTree().Paused = false;
		Hide();
	}

	private void OnMainMenu()
	{
		GetTree().Paused = false;
		GameManager.Instance.ElapsedTime = 0.0f;
		GetTree().ChangeSceneToFile("res://Scenes/CharacterSelect.tscn");
	}

	private void OnQuit()
	{
		GetTree().Quit();
	}
}
