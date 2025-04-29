using Godot;
using System;

public partial class AutoJodlerProjectile : WallbangProjectile
{
    [ExportSubgroup("Nodes")]
    [Export]
    public AnimatedSprite2D Sprite { get; set; }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(Velocity.X != 0)
        {
            Sprite.FlipV = Velocity.X < 0;
        }
    }
}
