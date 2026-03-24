using Godot;
using System;

public partial class MovingPlatformIce : Node2D
{
	[Export]
	public string animation = "";
	AnimationPlayer platform_anim;
	Timer timer;

	public override void _Ready()
	{
		timer = GetNode<Timer>("%Timer");
        platform_anim = GetNode<AnimationPlayer>("%Platform Move");
        platform_anim.Play(animation);
		timer.Timeout += PlayAnim;
        GD.Print(GetNode<Node2D>("%Moving Platform").Position);
    }

	private void PlayAnim()
	{
		platform_anim.Play(platform_anim.CurrentAnimation);
	}

	private void PauseAnim()
	{
		platform_anim.Pause();
		timer.Start(1.0);
	}
}
