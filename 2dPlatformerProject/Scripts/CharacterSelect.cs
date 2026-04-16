using Godot;

public partial class CharacterSelect : Control
{
	public override void _Ready()
	{
		GetNode<Button>("VBox/KnightButton").Pressed += OnKnightSelected;
		GetNode<Button>("VBox/FrogButton").Pressed += OnFrogSelected;
	}

	private void OnKnightSelected()
	{
		GameManager.Instance.SelectedCharacter = "Knight";
		GameManager.Instance.ElapsedTime = 0.0f;
		GetTree().ChangeSceneToFile("res://Scenes/game.tscn");
	}

	private void OnFrogSelected()
	{
		GameManager.Instance.SelectedCharacter = "Frog";
		GameManager.Instance.ElapsedTime = 0.0f;
		GetTree().ChangeSceneToFile("res://Scenes/game.tscn");
	}
}
