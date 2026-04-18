using Godot;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		GetNode<Button>("VBox/PlayButton").Pressed += OnPlay;
		GetNode<Button>("VBox/SettingsButton").Pressed += OnSettings;
		GetNode<Button>("VBox/QuitButton").Pressed += OnQuit;
	}

	private void OnPlay()
	{
		GetTree().ChangeSceneToFile("res://Scenes/CharacterSelect.tscn");
	}

	private void OnSettings()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Settings.tscn");
	}

	private void OnQuit()
	{
		GetTree().Quit();
	}
}
