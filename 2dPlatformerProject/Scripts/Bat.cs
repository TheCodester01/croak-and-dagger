using Godot;
using System;

public partial class Bat : Node2D
{
	private enum Mode
	{
		Recover,
		Target,
		Patrol,
	}

	private enum PatrolDirection
	{
		Left,
		Right
	}

	private AnimationPlayer animation_player;
	private AnimatedSprite2D animated_sprite;
	private PatrolDirection patrolDirection = PatrolDirection.Left;

	[Export]
	public CharacterBody2D frog_player;
	[Export]
	public CharacterBody2D knight_player;

	private CharacterBody2D player;

	[Export]
	public float target_speed = 50.0f;

	[Export]
	public float patrolSpeed = 50.0f;

	private Mode mode = Mode.Patrol;
	private Vector2 patrolPosition;
	private bool patrolFlipH;

	[Export]
	private Vector2 patrolLeft = new Vector2(-155.0f, 0.0f);

	[Export]
	private Vector2 patrolRight = new Vector2(0.0f, 0.0f);

	private float curAttackTimer = 0.0f;

	[Export]
	public float SecondsBetweenAttack = 1.5f;

	public override void _Ready()
	{
		if (GameManager.Instance.SelectedCharacter == "frog")
		{
			player = frog_player;
		}
		else
		{
			player = knight_player;
		}

		animation_player = GetNode<AnimationPlayer>("AnimationPlayer");
		animated_sprite = GetNode<AnimatedSprite2D>("Area2D/AnimatedSprite2D");

		patrolPosition = this.GlobalPosition;
		patrolFlipH = animated_sprite.FlipH;

		animated_sprite.Play("idle");

		patrolLeft += this.GlobalPosition;
		patrolRight += this.GlobalPosition;
	}

	public override void _Process(double delta)
	{
		// todo: when bat is really close, play attack animation
		if (curAttackTimer < SecondsBetweenAttack)
		{
			curAttackTimer += (float)delta;
		}

		if (this.GlobalPosition.DistanceTo(player.GlobalPosition) <= 150.0f)
		{
			if (this.GlobalPosition.DistanceTo(player.GlobalPosition) <= 50.0f)
			{
				this.Attack();
			}

			if (mode == Mode.Patrol)
			{
				patrolPosition = this.GlobalPosition;
				patrolFlipH = animated_sprite.FlipH;
			}

			if (mode == Mode.Patrol || mode == Mode.Recover)
			{
				mode = Mode.Target;
			}

			this.GlobalPosition = this.GlobalPosition.MoveToward(player.GlobalPosition, target_speed * (float)delta);

			if ((player.GlobalPosition.X - this.GlobalPosition.X) > 0)
			{
				animated_sprite.FlipH = true;
			}
			else
			{
				animated_sprite.FlipH = false;
			}
		}
		else
		{
			if (mode == Mode.Target)
			{
				mode = Mode.Recover;
			}

			if (mode == Mode.Recover)
			{
				if (this.GlobalPosition.DistanceTo(patrolPosition) <= 1.0f)
				{
					animated_sprite.FlipH = patrolFlipH;
					mode = Mode.Patrol;
				}
				else
				{
					this.GlobalPosition = this.GlobalPosition.MoveToward(patrolPosition, target_speed * (float)delta);

					if ((patrolPosition.X - this.GlobalPosition.X) > 0)
					{
						animated_sprite.FlipH = true;
					}
					else
					{
						animated_sprite.FlipH = false;
					}
				}
			}
			else if (mode == Mode.Patrol)
			{
				if (this.GlobalPosition == patrolLeft)
				{
					patrolDirection = PatrolDirection.Right;
				}
				else if (this.GlobalPosition == patrolRight)
				{
					patrolDirection = PatrolDirection.Left;
				}

				if (patrolDirection == PatrolDirection.Left)
				{
					this.GlobalPosition = this.GlobalPosition.MoveToward(patrolLeft, patrolSpeed * (float)delta);
					animated_sprite.FlipH = false;
				}
				else if (patrolDirection == PatrolDirection.Right)
				{
					this.GlobalPosition = this.GlobalPosition.MoveToward(patrolRight, patrolSpeed * (float)delta);
					animated_sprite.FlipH = true;
				}
			}
		}
	}

	private void Attack()
	{
		if (curAttackTimer >= SecondsBetweenAttack)
		{
			if (this.animated_sprite.FlipH)
			{
				animation_player.Play("attack_right");
			}
			else
			{
				animation_player.Play("attack_left");
			}

			curAttackTimer = 0.0f;
		}
	}
}
