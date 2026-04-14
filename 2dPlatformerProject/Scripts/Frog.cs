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

	[Export]
	public int SecondsBetweenDamage = 1;
    
	private float TimeSinceLastDamage = 0.0f;

	private bool TookDamage = false;

	AnimatedSprite2D anim_sprite;
	private AudioStreamPlayer _hurtSound;

	[Export]
	public HealthDisplay healthDisplay;

	[Export]
	public GameOver gameOver;

    public override void _Ready()
    {
        anim_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _hurtSound = GetNode<AudioStreamPlayer>("HurtSound");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

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

        Velocity = velocity;
        MoveAndSlide();
    }

    // Call this when collided with enemy
    public void TakeDamage() {
		if (!TookDamage && CurrentHearts > 0)
		{
			CurrentHearts--;
            anim_sprite.Play("hit");
            _hurtSound.Play();
            var heartSprite = healthDisplay.TakeDamage();
            TookDamage = true;

            if (CurrentHearts <= 0)
                ShowGameOverAfterAnimation(heartSprite);
        }
	}

    private async void ShowGameOverAfterAnimation(AnimatedSprite2D heartSprite)
    {
        await ToSignal(anim_sprite, AnimatedSprite2D.SignalName.AnimationFinished);
        if (heartSprite != null && heartSprite.IsPlaying())
            await ToSignal(heartSprite, AnimatedSprite2D.SignalName.AnimationFinished);
        gameOver.ShowGameOver();
    }

	// Call this when collided with heart item
	public bool AddHeart() {
		if (CurrentHearts < MaxHearts) {
			CurrentHearts++;
			healthDisplay.Recover();
            return true;
		}

		return false;
	}

	public void _on_area_2d_area_entered(Area2D area)
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
    }
}
