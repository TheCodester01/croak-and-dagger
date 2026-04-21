using Godot;

public partial class CharacterSelect : Control
{    
	public override void _Ready()
	{        
		GetNode<Button>("VBox/KnightButton").Pressed += OnKnightSelected;
		GetNode<Button>("VBox/FrogButton").Pressed += OnFrogSelected;
		GetNode<Button>("VBox/BackButton").Pressed += OnBack;
	}

	private void OnKnightSelected()
	{
		GameManager.Instance.SelectedCharacter = "Knight";
		GameManager.Instance.ElapsedTime = 0.0f;
		DisableButtons();
		PanThenLoad();
	}

	private void OnFrogSelected()
	{
		GameManager.Instance.SelectedCharacter = "Frog";
		GameManager.Instance.ElapsedTime = 0.0f;
		DisableButtons();
		PanThenLoad();
	}

	private void OnBack()
	{
		GetTree().ChangeSceneToPacked(SplashScreen.main_menu);
	}

	private void DisableButtons()
	{
		GetNode<Button>("VBox/KnightButton").Disabled = true;
		GetNode<Button>("VBox/FrogButton").Disabled = true;
		GetNode<Button>("VBox/BackButton").Disabled = true;
	}

	private void PanThenLoad()
	{
		var overlay = GetNode<ColorRect>("Overlay");
		var title = GetNode<Label>("Title");
		var vbox = GetNode<VBoxContainer>("VBox");

		var fadeTween = CreateTween().SetParallel(true);
		fadeTween.TweenProperty(overlay, "modulate:a", 0f, 0.4f);
		fadeTween.TweenProperty(title, "modulate:a", 0f, 0.4f);
		fadeTween.TweenProperty(vbox, "modulate:a", 0f, 0.4f);

		var cam = GetNode<Camera2D>(
			"SubViewportContainer/SubViewport/LevelPreview/Camera2D");
		var panTween = CreateTween();
		panTween.TweenProperty(cam, "position:y", 320f, 2.5f)
				.SetEase(Tween.EaseType.InOut)
				.SetTrans(Tween.TransitionType.Sine);
		panTween.TweenCallback(Callable.From(
			() => GetTree().ChangeSceneToPacked(SplashScreen.game_scene)));
	}
}
