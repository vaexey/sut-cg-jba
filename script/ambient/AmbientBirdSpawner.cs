using Godot;
using System;

public partial class AmbientBirdSpawner : Node2D
{
    [Export] public int Count { get; set; } = 0;

    [Export] public Node2D SpawnPoint { get; set; }

    public override void _Ready()
    {
        var scene = ResourceLoader.Load<PackedScene>($"res://script/ambient/AmbientBird.tscn");

        for (int i = 0; i < Count; i++)
        {
            var bird = scene.Instantiate();

            AddChild(bird);
        }
    }
}
