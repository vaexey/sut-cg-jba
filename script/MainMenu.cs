using Godot;
using System;

enum SubMenuView
{
    Abilities = 1,
    Attributes= 2,
};
public partial class MainMenu : Control
{

    private Control attrMenu;
    private Control abilityMenu;
    [Export] private TextEdit IpAddress;

    public override void _Ready()
    {
        abilityMenu = GetNode<Control>("SubmenuContainer/AbilitiesMenu");
        attrMenu = GetNode<Control>("SubmenuContainer/AttrMenu");
        // abilityMenu.Hide();
        // attrMenu.Show();
    }

    public void OnStartClick()
    {
        GetTree().ChangeSceneToFile("res://script/Arena.tscn");

        MultiplayerManager.Instance.HostGame();
    }

    public void OnJoinClick()
    {
        GetTree().ChangeSceneToFile("res://script/Arena.tscn");
        MultiplayerManager.Instance.JoinGame(IpAddress.Text);
    }

    public void OnAbilitiesClick()
    {
        abilityMenu.Show();
        attrMenu.Hide();
    }

    public void OnAttributesClick()
    {
        abilityMenu.Hide();
        attrMenu.Show();
    }

    public void OnQuitClick()
    {
        GetTree().Quit();
    }
}
