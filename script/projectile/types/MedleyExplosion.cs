using Godot;
using System;

public partial class MedleyExplosion : SimpleProjectile
{
    public override void _Ready()
    {
        base._Ready();

        Scale = new Vector2(0.1f, 0.1f);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var scale = (float)(1 - TimeLeft / DestroyAfterTime) * 0.9f + 0.1f;

        Scale = new Vector2(scale, scale);
    }
}
