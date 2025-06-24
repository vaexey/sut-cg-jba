using Godot;
using System;

public partial class PauseUI : CanvasLayer
{
    [Export] World World { get; set; }
    [Export] Control Container { get; set; }

    [Export] Control WaitingControl { get; set; }
    [Export] Control VictoryControl { get; set; }
    [Export] Control DefeatControl { get; set; }
    [Export] Control ResetControl { get; set; }

    public double PauseDelayTime { get; set; } = 5;
    public double UnpauseDelayTime { get; set; } = 0.75;

    public double PauseTime = 0;
    public double UnpauseTime = 0;

    public ulong LastProcess;

    public override void _Ready()
    {
        LastProcess = Time.GetTicksMsec();
    }

    protected double TimingFunction(double scale)
    {
        return Math.Pow(scale, 1 / 4.0);
    }

    public override void _Process(double delta)
    {
        // if (!Multiplayer.IsServer()) return;

        if (Input.IsActionJustPressed("game_pause"))
        {
            OnQuitButton();
        }

        var now = Time.GetTicksMsec();
        double realDelta = (now - LastProcess) / 1000.0;
        LastProcess = now;

        var shouldBeWaiting = ShouldBeWaiting();

        ResetControl.Visible = Multiplayer.IsServer()
            && shouldBeWaiting;

        if (shouldBeWaiting)
        {
            UnpauseTime = UnpauseDelayTime;
            Visible = true;

            if (!GetTree().Paused)
            {
                if (PauseTime <= 0)
                {
                    GetTree().Paused = true;
                    PauseTime = PauseDelayTime;
                    Container.Modulate = Colors.White;
                }
                else
                {
                    PauseTime = Mathf.MoveToward(PauseTime, 0, realDelta);

                    var scale = TimingFunction(1 - PauseTime / PauseDelayTime);

                    GD.Print($"Pausing: {scale * 100}%");
                    Engine.TimeScale = Math.Max(0.01, 1 - scale);
                    Container.Modulate = new Color(Colors.White, (float)scale);
                }
            }
        }
        else
        {
            PauseTime = PauseDelayTime;
            if (GetTree().Paused)
            {
                UnpauseTime = UnpauseDelayTime;
                GetTree().Paused = false;
            }
            else
            {
                if (UnpauseTime <= 0)
                {
                    Engine.TimeScale = 1;
                    Visible = false;
                }
                else
                {
                    UnpauseTime = Mathf.MoveToward(UnpauseTime, 0, realDelta);

                    var scale = TimingFunction(1 - UnpauseTime / UnpauseDelayTime);

                    GD.Print($"Unpausing: {scale * 100}%");
                    Engine.TimeScale = Math.Max(0.01, scale);
                    Container.Modulate = new Color(Colors.White, 1f - (float)scale);
                }
            }
        }
    }

    protected bool ShouldBeWaiting()
    {
        if (!World.PlayersPresent)
        {
            WaitingControl.Visible = true;
            VictoryControl.Visible = false;
            DefeatControl.Visible = false;
            return true;
        }

        if (World.LocalDefeat)
        {
            WaitingControl.Visible = false;
            VictoryControl.Visible = false;
            DefeatControl.Visible = true;
            return true;
        }

        if (World.LocalVictory)
        {
            WaitingControl.Visible = false;
            VictoryControl.Visible = true;
            DefeatControl.Visible = false;
            return true;
        }

        WaitingControl.Visible = false;
        VictoryControl.Visible = false;
        DefeatControl.Visible = false;
        return false;
    }

    public void OnQuitButton()
    {
        // Unpause();
        GetTree().Paused = false;
        // GetTree().ChangeSceneToFile("res://script/MainMenu.tscn");

        MultiplayerManager.Instance.QuitGame();
    }

    public void OnResetClick()
    {
        World.ResetWorld();
    }

    // private void Pause()
    // {
    //     if (!GetTree().Paused)
    //     {
    //         if (Time <= 0)
    //         {
    //             GetTree().Paused = true;
    //             Visible = true;
    //             Time = DelayTime;
    //         }
    //     }
    // }

    // private void Unpause()
    // {
    //     if (GetTree().Paused)
    //     {
    //         GetTree().Paused = false;
    //         Visible = false;

    //         if (Time <= 0)
    //         {
    //             Time = DelayTime;
    //         }
    //     }
    // }
}
