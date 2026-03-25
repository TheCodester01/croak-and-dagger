using Godot;
using System;

public partial class MovingPlatform : Node2D
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
