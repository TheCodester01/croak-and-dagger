using Godot;
using System;

public partial class Camera2d : Camera2D
{
	[Export]
    private Sprite2D backgroundSprite;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		var tex_size = backgroundSprite.Texture.GetSize() * backgroundSprite.Scale;

		var left = backgroundSprite.GlobalPosition.X - (tex_size.X / 2);
		var right = backgroundSprite.GlobalPosition.X + (tex_size.X / 2);

        var top = backgroundSprite.GlobalPosition.Y - (tex_size.Y / 2);
        var bottom = backgroundSprite.GlobalPosition.Y + (tex_size.Y / 2);

        LimitLeft = (int)left;
        LimitRight = (int)right;
        LimitTop = (int)top;
        LimitBottom = (int)bottom;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
