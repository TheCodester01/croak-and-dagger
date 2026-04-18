using Godot;

public partial class Settings : Control
{
	private HSlider _musicSlider;
	private HSlider _sfxSlider;

	public override void _Ready()
	{
		_musicSlider = GetNode<HSlider>("VBox/MusicSlider");
		_sfxSlider = GetNode<HSlider>("VBox/SfxSlider");

		_musicSlider.Value = GameManager.Instance.MusicVolume;
		_sfxSlider.Value = GameManager.Instance.SfxVolume;

		_musicSlider.ValueChanged += OnMusicVolumeChanged;
		_sfxSlider.ValueChanged += OnSfxVolumeChanged;
		GetNode<Button>("VBox/BackButton").Pressed += OnBack;
	}

	private void OnMusicVolumeChanged(double value)
	{
		GameManager.Instance.MusicVolume = (float)value;
	}

	private void OnSfxVolumeChanged(double value)
	{
		GameManager.Instance.SfxVolume = (float)value;
	}

	private void OnBack()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
	}
}
