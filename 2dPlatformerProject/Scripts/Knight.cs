using Godot;
using System;

public partial class Knight : Player
{
    public const float Speed = 300.0f;
    public const float SprintMultiplier = 1.5f;
    public const float JumpVelocity = -500.0f;
    bool is_sprinting = false;
    bool jumped = false;
    private int facing_dir = 1;

    public override void _Process(double delta)
    {
        if (!(anim_sprite.IsPlaying() && anim_sprite.Animation == "hit") && !IsInKnockback())
        {
            if (!IsOnFloor())
            {
                anim_sprite.Animation = "jump";
                anim_sprite.FlipH = facing_dir == -1;
                float offset_x = anim_sprite.FlipH ? -64 : 0;
                anim_sprite.Offset = new Vector2(offset_x, 0);
            }
            else if (Velocity.X != 0.0 && IsOnFloor())
            {
                anim_sprite.Animation = is_sprinting ? "run" : "walk";
                anim_sprite.FlipH = facing_dir == -1;
                float offset_x = anim_sprite.FlipH ? -64 : 0;
                anim_sprite.Offset = new Vector2(offset_x, 0);
            }
            else
            {
                anim_sprite.Animation = "idle";
            }

            anim_sprite.Play();
        }

        base._Process(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta); // Call Player's physics process first

        Vector2 direction = Input.GetVector("left", "right", "ui_up", "ui_down"); // Get the input direction and handle the movement/deceleration.
        Vector2 velocity = Velocity;
        is_sprinting = Input.IsActionPressed("sprint") && IsOnFloor();

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // If not standing still
        if (direction != Vector2.Zero)
        {
            velocity.X = direction.X * Speed;

            if (is_sprinting)
            {
                velocity.X *= SprintMultiplier;
            }
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        if (direction.X != 0)
        {
            facing_dir = direction.X > 0 ? 1 : -1;
        }

        // Handle Jump.
        if (jumped && IsOnFloor())
        {
            velocity.Y = is_sprinting ? JumpVelocity * 1.10f : JumpVelocity;
            jumped = false;
        }


        if (!IsInKnockback())
        {
            Velocity = velocity;
        }
        else
        {
            Velocity = GetKnockback();
        }

        MoveAndSlide();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("jump") && IsOnFloor())
        {
            jumped = true;
        }
        base._UnhandledInput(@event);
    }

}

