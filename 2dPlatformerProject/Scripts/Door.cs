using Godot;
using System;

public partial class Door : Area2D
{

    Timer timer;
    Label label;

    Game game = Game.Instance;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        label = GetNode<Label>("Label");

        BodyEntered += OnBodyEntered;
        //game = (Game)GetParent().GetParent();
    }
    public void OnBodyEntered(Node2D body)
	{

        if (body is Player)
        {
            if ((game.PlayerKeys == game.KeyCount))
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
