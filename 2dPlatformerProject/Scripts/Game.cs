using Godot;

public partial class Game : Node2D
{

    private int player_keys;
	public HUD hud;
	public Player selected_character { get; private set; }
	public int KeyCount { get; private set; }
	public int PlayerKeys {
		get { return player_keys; }

		set {
			player_keys = value;
			hud.UpdateKeyCount();
		}
	}
	static public float Sliding_Multiplier { get; private set; } = 10000.0f;

	public override void _Ready()
	{
		var frog = GetNode<Node2D>("Frog");
		var knight = GetNode<Node2D>("Knight");
		selected_character = GetNode<Player>($"{GameManager.Instance.SelectedCharacter}");
		Bat.player = selected_character;
		hud = GetNode<HUD>("HUD");

		if (GameManager.Instance.SelectedCharacter == "Frog")
		{
			knight.QueueFree();
		}
		else
		{
			frog.QueueFree();
		}

		foreach (Node2D node in GetNode<Node2D>("%Levels").GetChildren())
		{
			if(node.HasNode("Key"))
			{
				foreach (Node level_node in node.GetChildren())
				{
					if (level_node is Key)
					{
						KeyCount += 1;
					}
				}
			}
		}

		hud.UpdateKeyCount();
	}
}
