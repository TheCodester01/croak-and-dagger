using Godot;
using System;

public partial class Player : CharacterBody2D
{
    const int SecondsBetweenDamage = 1;

	[Export]
    private int SecondsBetweenFogDamage = 3;
    
	const float KnockbackVelocityY = -200.0f;

	[Export]
	private HealthDisplay healthDisplay;

	[Export]
	private GameOver gameOver;

	[Export]
	private Fog fog;

	public int Hearts { get; private set; } = HealthDisplay.MaxHearts;
	private Game game;
	protected AnimatedSprite2D anim_sprite;
	private AudioStreamPlayer _hurtSound;
	private bool TookDamage = false;
	private bool TookDamageFog = false;
	private float TimeSinceLastDamage = 0.0f;
	private float TimeSinceLastFogDamage = 0.0f;

	private Vector2 Knockback;
	private float KnockbackDurationSecs = 0f;

    public override void _Ready()
	{
		game = GetParent<Game>(); ;
		anim_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_hurtSound = GetNode<AudioStreamPlayer>("HurtSound");
	}

	public bool IsInKnockback()
	{
		return KnockbackDurationSecs > 0.0f;
	}

	public Vector2 GetKnockback()
    {
        return Knockback;
    }

    public void ApplyKnockback(Vector2 direction, float force, float durationSecs)
    {
        Knockback = direction * force;
        KnockbackDurationSecs = durationSecs;
    }

    public virtual bool TakeDamage(bool is_fog = false)
	{
		if (!(is_fog ? TookDamageFog : TookDamage) && Hearts > 0)
		{
			Hearts--;
			anim_sprite.Play("hit");
			_hurtSound.Play();
			var heartSprite = healthDisplay.TakeDamage();

			if (is_fog)
                TookDamageFog = true;
            else
                TookDamage = true;

			if (Hearts <= 0)
				ShowGameOverAfterAnimation(heartSprite);

			return true;
		}

		return false;
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
            if (TakeDamage())
            {
                var knockback_dir = (this.GlobalPosition - area.GlobalPosition).Normalized();
                ApplyKnockback(knockback_dir, 300.0f, 0.2f);
            }
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
        // Handle fog
        if (this.GlobalPosition.Y > fog.FogPositionY)
        {
			if (this.TakeDamage(true))
            {
                ApplyKnockback(new Vector2(0.0f, -1.0f), 300.0f, 0.15f);
			}
        }
        
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

        if (TookDamageFog)
        {
            TimeSinceLastFogDamage += (float)delta;

            if (TimeSinceLastFogDamage >= SecondsBetweenFogDamage)
            {
                TookDamageFog = false;
                TimeSinceLastFogDamage = 0.0f;
            }
        }

		if (KnockbackDurationSecs > 0.0f)
            KnockbackDurationSecs -= (float)delta;
		else
			Knockback = Vector2.Zero;
    }

	private async void ShowGameOverAfterAnimation(AnimatedSprite2D heartSprite)
	{
		await ToSignal(anim_sprite, AnimatedSprite2D.SignalName.AnimationFinished);
		if (heartSprite != null && heartSprite.IsPlaying())
			await ToSignal(heartSprite, AnimatedSprite2D.SignalName.AnimationFinished);
		gameOver.ShowGameOver();
	}
}