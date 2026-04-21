using Godot;
using System.Threading.Tasks;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
        TextureRect splash_img = GetNode<TextureRect>("SplashScreen");

		var tween = CreateTween();
		tween.TweenInterval(3.0f);
		tween.TweenProperty(splash_img, "modulate:a", 0f, 1.5f);

        tween.TweenCallback(Callable.From(() => splash_img.QueueFree()));

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
