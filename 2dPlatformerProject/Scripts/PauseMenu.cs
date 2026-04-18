using Godot;

public partial class PauseMenu : CanvasLayer
{
	private HSlider _musicSlider;
	private HSlider _sfxSlider;

	public override void _Ready()
	{
		Hide();
		ProcessMode = ProcessModeEnum.Always;

		_musicSlider = GetNode<HSlider>("Panel/VBox/MusicSlider");
		_sfxSlider = GetNode<HSlider>("Panel/VBox/SfxSlider");

		GetNode<Button>("Panel/VBox/ContinueButton").Pressed += OnContinue;
		GetNode<Button>("Panel/VBox/MainMenuButton").Pressed += OnMainMenu;
		GetNode<Button>("Panel/VBox/QuitButton").Pressed += OnQuit;
		_musicSlider.ValueChanged += OnMusicVolumeChanged;
		_sfxSlider.ValueChanged += OnSfxVolumeChanged;
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
		_musicSlider.Value = GameManager.Instance.MusicVolume;
		_sfxSlider.Value = GameManager.Instance.SfxVolume;
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
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}

	private void OnQuit()
	{
		GetTree().Quit();
	}

	private void OnMusicVolumeChanged(double value)
	{
		GameManager.Instance.MusicVolume = (float)value;
	}

	private void OnSfxVolumeChanged(double value)
	{
		GameManager.Instance.SfxVolume = (float)value;
	}
}
