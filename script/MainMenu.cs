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

    public override void _Ready()
    {
        this.abilityMenu = GetNode<Control>("SubmenuContainer/AbilitiesMenu");
        this.attrMenu = GetNode<Control>("SubmenuContainer/AttrMenu");
        this.abilityMenu.Hide();
        this.attrMenu.Show();
    }

    public void OnStartClick()
    {
        GetTree().ChangeSceneToFile("res://script/Arena.tscn");

        MultiplayerManager.Instance.HostGame();
    }

    public void OnJoinClick()
    {
        GetTree().ChangeSceneToFile("res://script/Arena.tscn");
        MultiplayerManager.Instance.JoinGame();
    }

    public void OnAbilitiesClick()
    {
        this.abilityMenu.Show();
        this.attrMenu.Hide();
    }

    public void OnAttributesClick()
    {
        this.abilityMenu.Hide();
        this.attrMenu.Show();
    }
}
