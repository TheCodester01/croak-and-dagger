using Godot;
using System;

public partial class Frog : CharacterBody2D
{
	[Export]
	public int CurrentHearts = 3;
	 
	[Export]
	public int MaxHearts = 3;
	
	[Export]
	public float JumpPower = 0.0f;
	
	[Export]
	public float JumpPowerMinimum = 0.3f;
	
	[Export]
	public int JumpPowerMaxSeconds =  1;

	[Export]
	public float JumpVelocity = -800.0f;

	private float JumpPowerElapsedTime = 0.0f;
	
	public const float HorizontalVelocity = 150.0f;

	// Call this when collided with enemy
	public void TakeDamage() {
		if (CurrentHearts > 0)
			CurrentHearts--;
	}
	
	// Call this when collided with heart item
	public void AddHeart() {
		if (CurrentHearts < MaxHearts) {
			CurrentHearts++;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		
		// Handle jump power
		if (IsOnFloor()) {
			velocity.X = 0;
			
  			if (Input.IsActionPressed("ui_accept")) {
				float t = this.JumpPowerElapsedTime / this.JumpPowerMaxSeconds;
				this.JumpPower = Mathf.Lerp(this.JumpPowerMinimum, 1.0f, t);
				
				this.JumpPowerElapsedTime += (float)delta;
			} else {
				this.JumpPowerElapsedTime = 0;
			}
		}
		
		// Handle Jump.
		if (Input.IsActionJustReleased("ui_accept") && IsOnFloor()) {
			velocity.Y = JumpVelocity * this.JumpPower;
			
			if (Input.IsActionPressed("ui_left")) {
				velocity.X = -HorizontalVelocity;
			} else if (Input.IsActionPressed("ui_right")) {
				velocity.X = HorizontalVelocity;
			}
		}
		
		AnimatedSprite2D anim_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		if (velocity.Y < 0)
		{
			anim_sprite.Animation = "jump";
		}
		else if (velocity.Y > 0) {
			anim_sprite.Animation = "fall";			
		}
		else
		{
			anim_sprite.Animation = "idle";
		}

		anim_sprite.FlipH = direction.X < 0.0;

		anim_sprite.Play();

		Velocity = velocity;
		MoveAndSlide();
	}
}
