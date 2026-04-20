using Godot;
using System;

public partial class Trajectory : Line2D
{
	[Export]
	private int MaxPoints = 100;

	private CharacterBody2D testCollision;
	private Vector2 linePos;

	private KinematicCollision2D collision;

	public enum Direction
	{
		Left, Right
	}

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		Refresh();
	}

    public void Refresh()
	{
        testCollision = GetNode<CharacterBody2D>("TestObject");
     
		ClearPoints();

		linePos = GetParent<CharacterBody2D>().GlobalPosition;

		testCollision.Velocity = Vector2.Zero;

		for (int i = 0; i < MaxPoints; i++)
        {
            AddPoint(linePos);
        }

		testCollision.GlobalPosition = linePos;

		collision = null;
		Visible = false;
    }

	public void _update_trajectory(Direction direction, float jumpForce, Vector2 gravity, double delta, float horizontalVelocity, Vector2 startPos)
	{
		linePos = startPos;
		testCollision.GlobalPosition = linePos;

		Vector2 velocity = testCollision.Velocity;
		
		velocity.Y = jumpForce;
        testCollision.Velocity = velocity;

        if (direction == Direction.Left)
        {
            velocity.X = -horizontalVelocity;
			testCollision.Velocity = velocity;
        } else
		{
			velocity.X = horizontalVelocity;
            testCollision.Velocity = velocity;
        }

		for (int i = 0; i < GetPointCount(); ++i)
		{
			if (collision == null)
			{
				velocity.Y += gravity.Y * (float)delta;
                testCollision.Velocity = velocity;
            }

			SetPointPosition(i, testCollision.Position);
			collision = testCollision.MoveAndCollide(velocity * (float)delta, false);
        }

		Visible = true;
    }

	public void Clear()
	{
		collision = null;
		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
