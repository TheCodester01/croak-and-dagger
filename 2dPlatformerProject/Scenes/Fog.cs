using Godot;
using System;

public partial class Fog : ParallaxBackground
{
	[Export]
	public float FogSpeedHorizontal = 0.1f;

	[Export]
	private int TotalFogTimeSecs = 120;

	[Export]
	private float TotalGameHeight = 1944.0f;

    private double elapsed = 0.0;
	private ShaderMaterial ShaderMaterial;

	public float FogPositionY = 0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        ShaderMaterial = (ShaderMaterial)((ColorRect)this.GetNode("ParallaxLayer/ColorRect")).Material;
        FogPositionY = this.Offset.Y + TotalGameHeight;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        elapsed += delta;

		double offset_ratio = Mathf.Lerp(1.0f, 0.0f, elapsed / TotalFogTimeSecs);

        ShaderMaterial.SetShaderParameter("offset_ratio", offset_ratio);

        FogPositionY = this.Offset.Y + TotalGameHeight * (float)offset_ratio;

        float translation = (float)ShaderMaterial.GetShaderParameter("curve_x_translation");
        ShaderMaterial.SetShaderParameter("curve_x_translation", translation + FogSpeedHorizontal * delta);
    }
}
