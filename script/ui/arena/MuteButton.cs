using Godot;
using System;

public partial class MuteButton : TextureButton
{

    public override void _Ready()
    {
        // SetMultiplayerAuthority(Multiplayer.GetUniqueId());

        Pressed += OnPressed;
    }

    protected void OnPressed()
    {
        var master = AudioServer.GetBusIndex("Master");
        AudioServer.SetBusMute(master, ButtonPressed);
    }
}
