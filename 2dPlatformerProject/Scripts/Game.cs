using Godot;

public partial class Game : Node2D
{
	public override void _Ready()
	{
		var frog = GetNode<Node2D>("Frog");
		var player = GetNode<Node2D>("Player");

		bool useKnight = GameManager.Instance.SelectedCharacter == "knight";

		player.Visible = useKnight;
		frog.Visible = !useKnight;
	}
}
