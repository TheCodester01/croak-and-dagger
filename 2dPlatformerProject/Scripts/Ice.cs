using Godot;
using System;

public partial class Ice : Node2D
{

    AnimationPlayer platform;
    public override void _Ready()
    {
        platform = GetNode<MovingPlatform>("%Moving Platform 1").GetNode<AnimationPlayer>("%Platform Move");
        Timer timer = GetNode<Timer>("%Timer");
        platform.Pause();
        timer.Timeout += Resume;
    }

    public void Resume()
    {
        platform.Play();
    }
}
