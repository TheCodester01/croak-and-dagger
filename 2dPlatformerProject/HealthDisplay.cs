using Godot;
using System;

public partial class HealthDisplay : CanvasLayer
{
    private BoxContainer hearts;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        hearts = GetNode<BoxContainer>("Hearts");
    }
    
    public AnimatedSprite2D TakeDamage()
    {
        for (int i = hearts.GetChildCount() - 1; i >= 0; i--)
        {
            var heart = hearts.GetChild<Control>(i);
            var heart_anim_sprite = heart.GetNode<AnimatedSprite2D>("Heart");

            if (heart_anim_sprite.Animation == "idle" || heart_anim_sprite.Animation == "recover")
            {
                heart_anim_sprite.Play("lost");
                return heart_anim_sprite;
            }
        }
        return null;
    }

    public void Recover()
    {
        for (int i = 0; i < hearts.GetChildCount(); ++i)
        {
            var heart = hearts.GetChild<Control>(i);
            var heart_anim_sprite = heart.GetNode<AnimatedSprite2D>("Heart");

            if (heart_anim_sprite.Animation == "lost")
            {
                heart_anim_sprite.Play("recover");
                break;
            }
        }
    }
}
