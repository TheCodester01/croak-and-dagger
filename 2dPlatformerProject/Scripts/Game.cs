using Godot;

public partial class Game : Node2D
{

	int key_count;

	public override void _Ready()
	{
		var frog = GetNode<Node2D>("Frog");
		var player = GetNode<Node2D>("Knight");

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
				key_count += 1;
			}
		}

		GD.Print(key_count);
	}
}
