using Godot;
using System;

public partial class MainMenu : Control
{

    public void OnStartClick()
    {
        GetTree().ChangeSceneToFile("res://script/Arena.tscn");
    }
}
