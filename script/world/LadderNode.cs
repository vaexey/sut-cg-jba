using Godot;
using System;

public partial class LadderNode : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        if (body.GetType().IsAssignableTo(typeof(Player)))
        {
            var player = (Player)body;

            player.Entity.IsOnLadder++;
            GD.Print("player on");
        }
    }

    public void OnBodyExited(Node2D body)
    {
        if (body.GetType().IsAssignableTo(typeof(Player)))
        {
            var player = (Player)body;

            player.Entity.IsOnLadder--;
            GD.Print("player off");
        }
    }
}
