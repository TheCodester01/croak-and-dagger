using Godot;

public partial class GameOver : CanvasLayer
{
    public override void _Ready()
    {
        Hide();
        ProcessMode = ProcessModeEnum.Always;

        GetNode<Button>("Panel/VBox/TryAgainButton").Pressed += OnTryAgain;
        GetNode<Button>("Panel/VBox/MainMenuButton").Pressed += OnMainMenu;
    }

    public void ShowGameOver()
    {
        GetTree().Paused = true;
        Show();
    }

    private void OnTryAgain()
    {
        GetTree().Paused = false;
        GameManager.Instance.ElapsedTime = 0.0f;
        GetTree().ReloadCurrentScene();
    }

    private void OnMainMenu()
    {
        GetTree().Paused = false;
        GameManager.Instance.ElapsedTime = 0.0f;
        GetTree().ChangeSceneToFile("res://Scenes/CharacterSelect.tscn");
    }
}
