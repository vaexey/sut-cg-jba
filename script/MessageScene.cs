using Godot;
using System;

public partial class MessageScene : Control
{
    [ExportSubgroup("Settings")]
    [Export]
    public string Title
    {
        get => TitleLabel.Text;
        set => TitleLabel.Text = value;
    }

    [Export]
    public string Message
    {
        get => MessageLabel.Text;
        set => MessageLabel.Text = value;
    }

    [ExportSubgroup("Nodes")]
    [Export] public Label TitleLabel { get; set; }
    [Export] public Label MessageLabel { get; set; }

    public static MessageScene Instantiate(string message = null, string title = null)
    {
        var packed = ResourceLoader.Load<PackedScene>($"res://script/MessageScene.tscn");
        var instance = packed.Instantiate<MessageScene>();

        instance.Title = title ?? instance.Title;
        instance.Message = message ?? instance.Message;

        return instance;
    }


    // public static void Show(SceneTree tree, string message = null, string title = null)
    // {
    //     var old = tree.CurrentScene;
    //     var msg = Instantiate(message, title);

    //     tree.Root.AddChild(msg);
    //     tree.Root.RemoveChild(old);
    //     tree.CurrentScene = msg;
    // }

    public void OnQuit()
    {
        GetTree().ChangeSceneToFile("res://script/MainMenu.tscn");
    }
}
