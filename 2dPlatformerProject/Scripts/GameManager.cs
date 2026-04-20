using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }
	public string SelectedCharacter { get; set; } = "";
	public float ElapsedTime { get; set; } = 0.0f;
	public bool IsPaused { get; set; } = false;

	private AudioStreamPlayer _music;

	public override void _Ready()
	{
		Instance = this;

		_music = new AudioStreamPlayer();
		AddChild(_music);
		_music.Finished += LoopMusic;
		_music.Stream = GD.Load<AudioStream>("res://Assets/Audio/moodmode-8-bit-arcade-138828.mp3");
		_music.VolumeDb = -10f;
		_music.Play();
	}

    private void LoopMusic()
	{
		_music.Play();
	}
}
