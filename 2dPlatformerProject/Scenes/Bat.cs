using Godot;
using System;

public partial class Bat : CharacterBody2D
{
	[Export]
	public AnimationPlayer animation_player;
	
	public override void _Ready()
	{
		animation_player.Play("move");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		
		

		Velocity = velocity;
		MoveAndSlide();
	}
}
