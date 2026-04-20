using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }
	public string SelectedCharacter { get; set; } = "";
	public float ElapsedTime { get; set; } = 0.0f;
	public bool IsPaused { get; set; } = false;

	private float _musicVolume = 1.0f;
	public float MusicVolume
	{
		get => _musicVolume;
		set
		{
			_musicVolume = value;
			if (_music != null)
				_music.VolumeDb = value == 0f ? -80f : Mathf.LinearToDb(value);
		}
	}

	private float _sfxVolume = 1.0f;
	public float SfxVolume
	{
		get => _sfxVolume;
		set => _sfxVolume = value;
	}

	private AudioStreamPlayer _music;

	public override void _Ready()
	{
		Instance = this;

		_music = new AudioStreamPlayer();
		AddChild(_music);
		_music.Finished += LoopMusic;
		_music.Stream = GD.Load<AudioStream>("res://Assets/Audio/moodmode-8-bit-arcade-138828.mp3");
		_music.VolumeDb = Mathf.LinearToDb(_musicVolume);
		_music.Play();
	}

    private void LoopMusic()
	{
		_music.Play();
	}
}
