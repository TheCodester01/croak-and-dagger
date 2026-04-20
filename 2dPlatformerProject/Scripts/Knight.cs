using Godot;
using System;

public partial class Knight : Player
{
    public const float Speed = 300.0f;
    public const float SprintMultiplier = 1.5f;
    public const float JumpVelocity = -500.0f;
    bool is_sprinting = false;
    public bool jumped = false;
    private int facing_dir = 1;

    private float acceleration = 1400.0f;
    private float friction = 1800.0f;

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
        velocity.Y += GetGravity().Y * (float)delta; // Apply Gravity at all times

        if (IsOnFloor())
        {

            if (GetFloorAngle() > .69f && (Position.Y < 0 && Position.Y > -648))
            {
                FloorStopOnSlope = false;
                Vector2 normal = GetFloorNormal();
                Vector2 slope_dir = new Vector2(normal.Y, -normal.X); // Gets perpendicular vector to normal (parallel to slope)
                if (slope_dir.Dot(Vector2.Down) < 0) // Ensures that slope_dir is downhill and not uphill
                {
                    slope_dir = -slope_dir;
                }
                velocity += slope_dir * Game.Sliding_Multiplier * (float)delta;
            }
            else
            {
                FloorStopOnSlope = true;
            }

            is_sprinting = Input.IsActionPressed("sprint");
            if (jumped)
            {

                velocity.Y = is_sprinting ? JumpVelocity * 1.10f : JumpVelocity;
                jumped = false;
            }

        }

        // If not standing still
        if (direction != Vector2.Zero)
        {
            float target_x = direction.X * Speed;

            if (is_sprinting)
            {
                target_x *= SprintMultiplier;
            }

            velocity = velocity.MoveToward(new Vector2(target_x, velocity.Y), acceleration * (float)delta);
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, friction * (float)delta);
        }

        if (direction.X != 0)
        {
            facing_dir = direction.X > 0 ? 1 : -1;
        }

        // Handle Jump.

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

