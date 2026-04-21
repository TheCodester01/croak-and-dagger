using Godot;
using System;

public partial class SplashScreen : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        TextureRect image = GetNode<TextureRect>("Image");

        var tween = CreateTween();
        tween.TweenInterval(3.0f);
        tween.TweenProperty(image, "modulate:a", 0f, 1.5f);

        tween.TweenCallback(Callable.From(() => GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn")));
    }
}
