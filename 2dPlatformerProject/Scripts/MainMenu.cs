using Godot;
using System.Threading.Tasks;

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
		GetTree().ChangeSceneToPacked(SplashScreen.character_select_scene);
	}

	private void OnSettings()
	{
		GetTree().ChangeSceneToPacked(SplashScreen.settings_scene);
	}

	private void OnQuit()
	{
		GetTree().Quit();
	}
}
