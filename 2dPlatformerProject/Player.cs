using Godot;
using System;

public partial class Player : CharacterBody2D
{
	const int SecondsBetweenDamage = 1;
	const float KnockbackVelocityY = -200.0f;

	[Export]
	private HealthDisplay healthDisplay;

	[Export]
	private GameOver gameOver;

	public int Hearts { get; private set; } = HealthDisplay.MaxHearts;
	private Game game;
	protected AnimatedSprite2D anim_sprite;
	private AudioStreamPlayer _hurtSound;
	private bool TookDamage = false;
	private float TimeSinceLastDamage = 0.0f;

	public override void _Ready()
	{
		game = GetParent<Game>(); ;
		anim_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_hurtSound = GetNode<AudioStreamPlayer>("HurtSound");
	}

	public bool IsInKnockback()
	{
		return TookDamage && Velocity.Length() != 0.0f;
	}

	public virtual void OnTakeDamage()
	{
		if (Hearts > 0)
		{
			Velocity = new Vector2(Velocity.X * -0.25f, Math.Min(Velocity.Y, KnockbackVelocityY));
		}


	}

	public virtual void TakeDamage()
	{
		if (!TookDamage && Hearts > 0)
		{
			Hearts--;
			anim_sprite.Play("hit");
			_hurtSound.Play();
			var heartSprite = healthDisplay.TakeDamage();
			TookDamage = true;

			if (Hearts <= 0)
				ShowGameOverAfterAnimation(heartSprite);

			OnTakeDamage();
		}
	}
	public virtual bool AddHeart()
	{
		if (Hearts < HealthDisplay.MaxHearts)
		{
			Hearts++;
			healthDisplay.Recover();
			return true;
		}

		return false;
	}

	public virtual void _on_area_2d_area_entered(Area2D area)
	{
		if (area.IsInGroup("enemy"))
		{
			TakeDamage();
		}
		else if (area.IsInGroup("heart"))
		{
			if (AddHeart())
				area.QueueFree(); // This deletes the item node from the scene
		}
		else if (area.IsInGroup("key"))
		{
			area.QueueFree();
			game.PlayerKeys += 1;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// Handle damage cooldown
		if (TookDamage)
		{
			TimeSinceLastDamage += (float)delta;

			if (TimeSinceLastDamage >= SecondsBetweenDamage)
			{
				TookDamage = false;
				TimeSinceLastDamage = 0.0f;
			}
		}
	}

	private async void ShowGameOverAfterAnimation(AnimatedSprite2D heartSprite)
	{
		await ToSignal(anim_sprite, AnimatedSprite2D.SignalName.AnimationFinished);
		if (heartSprite != null && heartSprite.IsPlaying())
			await ToSignal(heartSprite, AnimatedSprite2D.SignalName.AnimationFinished);
		gameOver.ShowGameOver();
	}
}