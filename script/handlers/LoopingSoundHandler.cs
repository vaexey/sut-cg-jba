using Godot;
using System;

[Tool]
public partial class LoopingSoundHandler : AudioStreamPlayer2D
{
    [ExportCategory("Settings")]
    [Export]
    public bool KeepPlaying { get; set; }
    [Export]
    public double IntervalMin { get; set; } = 1;

    [Export]
    public double IntervalMax { get; set; } = 1;

    private double Timeout = 0;

    public override void _Process(double delta)
    {
        if(KeepPlaying)
        {
            Timeout -= delta;

            if(Timeout <= 0)
            {
                Play();

                Timeout = GD.RandRange(IntervalMin, IntervalMax);
            }
        } else {
            Timeout = -1;
        }
    }

    public void SetKeepPlaying(bool keep)
    {
        KeepPlaying = keep;
    }
}
