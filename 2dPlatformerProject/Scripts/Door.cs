using Godot;
using System;

public partial class Door : Area2D
{

    Timer timer;
    Label label;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        label = GetNode<Label>("Label");

        BodyEntered += OnBodyEntered;
    }
    public void OnBodyEntered(Node2D body)
	{
        if (body is Player)
        {
            if ((Game.Instance.PlayerKeys == Game.Instance.KeyCount))
            {
                GetTree().ChangeSceneToFile("res://Scenes/win_screen.tscn");
            }
            else
            {
                label.Visible = true;
                timer.Start();
            }
        }
    }
}
