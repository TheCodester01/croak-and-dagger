using Godot;

public partial class Game : Node2D
{
	public HUD hud;
    private int player_keys;
	public int KeyCount { get; private set; }
	public int PlayerKeys {
		get { return player_keys; }

		set {
			player_keys = value;
			hud.UpdateKeyCount();
		}
	}

	public override void _Ready()
	{
		var frog = GetNode<Node2D>("Frog");
		var player = GetNode<Node2D>("Knight");
		hud = GetNode<HUD>("HUD");

		if (GameManager.Instance.SelectedCharacter == "frog")
		{
			player.QueueFree();
		}
		else
		{
			frog.QueueFree();
		}

		foreach (Node2D node in GetNode<Node2D>("%Levels").GetChildren())
		{
			if(node.HasNode("Key"))
			{
				KeyCount += 1;
			}
		}

		hud.UpdateKeyCount();
	}
}
