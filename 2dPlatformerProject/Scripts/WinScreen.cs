using Godot;
using System;

public partial class WinScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        int totalSeconds = (int)GameManager.Instance.ElapsedTime;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        GetNode<Label>("Time").Text += $"{minutes:00}:{seconds:00}";
        GetNode<Button>("PlayAgain").Pressed += OnTryAgain;
        GetNode<Button>("Quit").Pressed += OnMainMenu;
	}

    private void OnTryAgain()
    {
        GetTree().Paused = false;
        GameManager.Instance.ElapsedTime = 0.0f;
        GetTree().ChangeSceneToFile("res://Scenes/CharacterSelect.tscn");
    }

    private void OnMainMenu()
    {
        GetTree().Paused = false;
        GameManager.Instance.ElapsedTime = 0.0f;
        GetTree().Quit();
    }
}
