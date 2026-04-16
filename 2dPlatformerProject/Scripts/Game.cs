using Godot;

public partial class Game : Node2D
{
    private int player_keys;
	public HUD hud;
	public CharacterBody2D selected_character { get; private set; }
	public int KeyCount { get; private set; }

	private Bat bat;
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
		var knight = GetNode<Node2D>("Knight");
		selected_character = GetNode<CharacterBody2D>($"{GameManager.Instance.SelectedCharacter}");
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
				KeyCount += 1;
			}
		}

		hud.UpdateKeyCount();
	}
}
