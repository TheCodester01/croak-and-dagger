using Godot;
using System;

public partial class SplashScreen : Node
{
    public static readonly PackedScene main_menu = GD.Load<PackedScene>("res://Scenes/MainMenu.tscn");
    public static readonly PackedScene game_scene = GD.Load<PackedScene>("res://Scenes/game.tscn");
    public static readonly PackedScene character_select_scene = GD.Load<PackedScene>("res://Scenes/CharacterSelect.tscn");
    public static readonly PackedScene settings_scene = GD.Load<PackedScene>("res://Scenes/Settings.tscn");

    public override void _Ready()
	{
        main_menu.Instantiate();
        game_scene.Instantiate();
        character_select_scene.Instantiate();
        settings_scene.Instantiate();

        TextureRect image = GetNode<TextureRect>("Image");

        var tween = CreateTween();
        tween.TweenInterval(3.0f);
        tween.TweenProperty(image, "modulate:a", 0f, 1.5f);

        tween.TweenCallback(Callable.From(() => GetTree().ChangeSceneToPacked(main_menu)));
    }
}
