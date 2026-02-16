using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float SprintMultiplier = 1.5f;
	public const float JumpVelocity = -500.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        bool is_sprinting = Input.IsActionPressed("sprint") && IsOnFloor();

        if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;

            if (is_sprinting)
                velocity.X *= SprintMultiplier;
        }
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        AnimatedSprite2D anim_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (velocity.Y != 0.0)
        {
            anim_sprite.Animation = "jump";
        }
        else if (velocity.X != 0.0)
        {
            anim_sprite.Animation = (is_sprinting) ? "run" : "walk";
            anim_sprite.FlipH = velocity.X < 0.0;

			float offset_x = anim_sprite.FlipH ? -64 : 0;
            anim_sprite.Offset = new Vector2(offset_x, 0);
        }
        else
        {
            anim_sprite.Animation = "idle";
        }

        anim_sprite.Play();

        Velocity = velocity;
		MoveAndSlide();
	}
}
