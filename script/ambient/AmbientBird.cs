using Godot;
using System;

public partial class AmbientBird : CharacterBody2D
{
    public AmbientBirdSpawner Spawner => GetParent<AmbientBirdSpawner>();

    [Export] public AnimatedSprite2D Sprite { get; set; }

    public override void _Ready()
    {
        Position = Spawner.SpawnPoint.Position;
    }

    public override void _Process(double delta)
    {
        if (Velocity.X != 0)
        {
            Sprite.FlipH = Velocity.X > 0;

            Sprite.Play("fly");
        }
        else
        {
            Sprite.Play("idle");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = new(10, 0);
        MoveAndSlide();
    }
}
