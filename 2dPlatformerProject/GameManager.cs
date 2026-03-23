using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	public string SelectedCharacter { get; set; } = "";
	public float ElapsedTime { get; set; } = 0.0f;
	public bool IsPaused { get; set; } = false;

	public override void _Ready()
	{
		Instance = this;
	}
}
