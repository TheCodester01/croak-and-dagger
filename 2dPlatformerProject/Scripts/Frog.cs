using Godot;
using System;

public partial class Frog : Player
{
    private Vector2 JumpPower = Vector2.Zero;

    [Export]
    public float JumpScaleSpeed = 200.0f;

    [Export]
    public float JumpPowerMinimum = 0.3f;

    [Export]
    public int JumpPowerMaxSeconds = 1;

    [Export]
    public float JumpVelocity = -800.0f;

    private float JumpPowerElapsedTime = 0.0f;

    public const float HorizontalVelocity = 400.0f;

    [Export]
    public float MomentumConservation = 0.3f;

    private Trajectory trajectory;

    public override void _Ready()
    {
        base._Ready();
        trajectory = GetNode<Trajectory>("Trajectory");
        JumpPower = new Vector2(0.0f, JumpPowerMinimum * JumpVelocity);
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

            /*if (Input.IsActionPressed("jump"))
            {
                float t = Mathf.Clamp(this.JumpPowerElapsedTime / this.JumpPowerMaxSeconds, 0.0f, 1.0f);
                this.JumpPower = Mathf.Lerp(this.JumpPowerMinimum, 1.0f, t);

                this.JumpPowerElapsedTime += (float)delta;
            }
            else
            {
                this.JumpPowerElapsedTime = 0;
            }*/

            bool jump_power_changed = false;

            if (Input.IsActionPressed("left"))
            {
                this.JumpPower = this.JumpPower.MoveToward(new Vector2(-HorizontalVelocity, JumpPower.Y), JumpScaleSpeed * (float)delta);
                jump_power_changed = true;
            }

            if (Input.IsActionPressed("right"))
            {
                this.JumpPower = this.JumpPower.MoveToward(new Vector2(HorizontalVelocity, JumpPower.Y), JumpScaleSpeed * (float)delta);
                jump_power_changed = true;
            }

            if (Input.IsActionPressed("ui_up"))
            {
                this.JumpPower = this.JumpPower.MoveToward(new Vector2(JumpPower.X, JumpVelocity), JumpScaleSpeed * (float)delta);
                jump_power_changed = true;
            }

            if (Input.IsActionPressed("ui_down"))
            {
                this.JumpPower = this.JumpPower.MoveToward(new Vector2(JumpPower.X, JumpPowerMinimum * JumpVelocity), JumpScaleSpeed * (float)delta);
                jump_power_changed = true;
            }

            if (jump_power_changed && JumpPower.X != 0.0f)
            {
                trajectory._update_trajectory(JumpPower.Y, GetGravity(), delta, JumpPower.X, GlobalPosition);
            }
        }

        // Handle Jump.
        if (Input.IsActionJustReleased("jump") && IsOnFloor())
        {
            velocity = this.JumpPower;
            this.JumpPower = new Vector2(0.0f, JumpPowerMinimum * JumpVelocity);
            /*velocity.Y = JumpVelocity * this.JumpPower;

            if (Input.IsActionPressed("left"))
            {
                velocity.X = -HorizontalVelocity;
            }
            else if (Input.IsActionPressed("right"))
            {
                velocity.X = HorizontalVelocity;
            }*/

            trajectory.Clear();
        }

        /*if (Input.IsActionPressed("jump") && IsOnFloor())
        {
            if (Input.IsActionPressed("left"))
            {
                trajectory._update_trajectory(Trajectory.Direction.Left, JumpVelocity * this.JumpPower, GetGravity(), delta, HorizontalVelocity, GlobalPosition);
            }
            else if (Input.IsActionPressed("right"))
            {
                trajectory._update_trajectory(Trajectory.Direction.Right, JumpVelocity * this.JumpPower, GetGravity(), delta, HorizontalVelocity, GlobalPosition);
            }
        }*/

        Vector2 direction = Input.GetVector("left", "right", "ui_up", "ui_down");

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

        Vector2 preVelocity = Velocity;
        MoveAndSlide();

        if (!IsInKnockback())
        {
            for (int i = 0; i < GetSlideCollisionCount(); i++)
            {
                var collision = GetSlideCollision(i);
                Vector2 normal = collision.GetNormal();

                if (normal.Dot(Vector2.Up) > 0.7f) continue; // ignore floor

                Vector2 lostVelocity = preVelocity - Velocity;

                if (Mathf.Abs(normal.X) > 0.7f) // wall hit — redirect horizontal speed upward
                {
                    Velocity += new Vector2(0, -Mathf.Abs(lostVelocity.X) * MomentumConservation);
                }
                else if (normal.Y > 0.7f) // ceiling hit — redirect upward speed horizontally
                {
                    float sign = preVelocity.X >= 0 ? 1f : -1f;
                    Velocity += new Vector2(sign * Mathf.Abs(lostVelocity.Y) * MomentumConservation, 0);
                }
            }
        }
    }
}
