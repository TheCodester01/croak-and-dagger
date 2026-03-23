using Godot;
using System;

public partial class Ice : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationPlayer moving_platform = GetNode("%Moving Platform").GetNode<AnimationPlayer>("%Platform Animation");
		moving_platform.Play("platform_move");
	}
}
