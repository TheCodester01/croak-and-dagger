using Godot;
using System;

public partial class Frog : Player
{

    [Export]
    public float JumpPower = 0.0f;

    [Export]
    public float JumpPowerMinimum = 0.3f;

    [Export]
    public int JumpPowerMaxSeconds = 1;

    [Export]
    public float JumpVelocity = -700.0f;

    private float JumpPowerElapsedTime = 0.0f;

    public const float HorizontalVelocity = 200.0f;

    private Trajectory trajectory;

    public override void _Ready()
    {
        base._Ready();
        trajectory = GetNode<Trajectory>("Trajectory");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta); // Call Player's physics process first

        Vector2 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }

        // Handle jump power
        if (IsOnFloor())
        {
            velocity.X = 0;

            if (Input.IsActionPressed("ui_accept"))
            {
                float t = Mathf.Clamp(this.JumpPowerElapsedTime / this.JumpPowerMaxSeconds, 0.0f, 1.0f);
                this.JumpPower = Mathf.Lerp(this.JumpPowerMinimum, 1.0f, t);

                this.JumpPowerElapsedTime += (float)delta;
            }
            else
            {
                this.JumpPowerElapsedTime = 0;
            }
        }

        // Handle Jump.
        if (Input.IsActionJustReleased("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity * this.JumpPower;

            if (Input.IsActionPressed("ui_left"))
            {
                velocity.X = -HorizontalVelocity;
            }
            else if (Input.IsActionPressed("ui_right"))
            {
                velocity.X = HorizontalVelocity;
            }

            trajectory.Clear();
        }

        if (Input.IsActionPressed("ui_accept") && IsOnFloor())
        {
            if (Input.IsActionPressed("ui_left"))
            {
                trajectory._update_trajectory(Trajectory.Direction.Left, JumpVelocity * this.JumpPower, GetGravity(), delta, HorizontalVelocity, GlobalPosition);
            }
            else if (Input.IsActionPressed("ui_right"))
            {
                trajectory._update_trajectory(Trajectory.Direction.Right, JumpVelocity * this.JumpPower, GetGravity(), delta, HorizontalVelocity, GlobalPosition);
            }
        }

        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        if (!(anim_sprite.IsPlaying() && anim_sprite.Animation == "hit"))
        {
            if (velocity.Y < 0)
            {
                anim_sprite.Animation = "jump";
            }
            else if (velocity.Y > 0)
            {
                anim_sprite.Animation = "fall";
            }
            else
            {
                anim_sprite.Animation = "idle";
            }

            anim_sprite.FlipH = direction.X < 0.0;

            anim_sprite.Play();
        }

        if (!IsInKnockback())
            Velocity = velocity;
        else
            Velocity = GetKnockback();

        MoveAndSlide();
    }
}
