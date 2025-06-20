using Godot;
using System;

public partial class PauseUI : CanvasLayer
{
    // [Export] World World { get; set; }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("game_pause"))
        {
            Pause();
        }

    }

    public void OnResumeButton()
    {
        Unpause();
    }

    public void OnQuitButton()
    {
        Unpause();
        GetTree().ChangeSceneToFile("res://script/MainMenu.tscn");
    }

    private void Pause()
    {
        GetTree().Paused = true;
        Visible = true;
    }

    private void Unpause()
    {
        GetTree().Paused = false;
        Visible = false;
    }
}
