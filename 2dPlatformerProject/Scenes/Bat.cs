using Godot;
using System;

public partial class Bat : Area2D
{
	[Export]
	public AnimationPlayer animation_player;
	
	public override void _Ready()
	{
		animation_player.Play("move");
	}
}
