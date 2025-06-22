using Godot;
using System;

public partial class RemoteInputHandler : InputHandler
{
    [Export] MultiplayerSynchronizer Sync { get; set; }

    protected class RIHSemaphore
    {
        bool T = false;

        public void Trigger()
        {
            T = true;
        }

        public bool Check()
        {
            return T;
            // if (T)
            // {
            //     T = false;
            //     return true;
            // }

            // return false;
        }

        public void Clear()
        {
            T = false;
        }
    }

    protected void Set(RIHSemaphore sem)
    {
        sem.Trigger();
    }

    public override void _Ready()
    {
        base._Ready();

        var id = GetParent().GetParent<Player>().Id;
        SetMultiplayerAuthority((int)id);
        // Sync.SetMultiplayerAuthority((int)id);

        // GD.Print(id);

        // if (GetMultiplayerAuthority() != Multiplayer.GetUniqueId())
        // {
        //     SetProcess(false);
        //     SetPhysicsProcess(false);
        // }
    }

    public virtual void ResetInputSemaphores()
    {
        if (Multiplayer.IsServer())
        {
            JumpPressed.Clear();
            JumpReleased.Clear();
            AbilityBasic.Clear();
            AbilityBasicPrevious.Clear();
            AbilityBasicNext.Clear();
            AbilityComplex1.Clear();
            AbilityComplex2.Clear();
            AbilityComplex3.Clear();
            AbilityGodlike.Clear();
            
            AbilityIndex = -1;
        }
    }

    public override void _Process(double delta)
    {
        // ResetInputSemaphores();

        if (GetMultiplayerAuthority() != Multiplayer.GetUniqueId())
            return;

        base._Process(delta);

        int idx = base.GetAbilityBasicIndex();
        if (idx != -1)
            AbilityIndex = idx;

        if (Input.IsActionJustPressed("move_jump"))
                Rpc(MethodName.SetJumpPressed);
        if (Input.IsActionJustReleased("move_jump"))
            Rpc(MethodName.SetJumpReleased);
        if (Input.IsActionJustPressed("ability_basic"))
            Rpc(MethodName.SetAbilityBasic);
        if (Input.IsActionJustPressed("ability_basic_prev"))
            Rpc(MethodName.SetAbilityBasicPrevious);
        if (Input.IsActionJustPressed("ability_basic_next"))
            Rpc(MethodName.SetAbilityBasicNext);
        if (Input.IsActionJustPressed("ability_complex1"))
            Rpc(MethodName.SetAbilityComplex1);
        if (Input.IsActionJustPressed("ability_complex2"))
            Rpc(MethodName.SetAbilityComplex2);
        if (Input.IsActionJustPressed("ability_complex3"))
            Rpc(MethodName.SetAbilityComplex3);
        if (Input.IsActionJustPressed("ability_godlike"))
            Rpc(MethodName.SetAbilityGodlike);
    }

    protected RIHSemaphore JumpPressed = new();
    protected RIHSemaphore JumpReleased = new();
    protected RIHSemaphore AbilityBasic = new();
    protected RIHSemaphore AbilityBasicPrevious = new();
    protected RIHSemaphore AbilityBasicNext = new();
    protected RIHSemaphore AbilityComplex1 = new();
    protected RIHSemaphore AbilityComplex2 = new();
    protected RIHSemaphore AbilityComplex3 = new();
    protected RIHSemaphore AbilityGodlike = new();

    protected int AbilityIndex = -1;

    [Rpc(CallLocal = true)]
    protected void SetJumpPressed() => Set(JumpPressed);
    public override bool GetJumpPressed() => JumpPressed.Check();

    [Rpc(CallLocal = true)]
    protected void SetJumpReleased() => Set(JumpReleased);
    public override bool GetJumpReleased() => JumpReleased.Check();

    [Rpc(CallLocal = true)]
    protected void SetAbilityBasic() => Set(AbilityBasic);
    public override bool GetAbilityBasic() => AbilityBasic.Check();

    [Rpc(CallLocal = true)]
    protected void SetAbilityBasicPrevious() => Set(AbilityBasicPrevious);
    public override bool GetAbilityBasicPrevious() => AbilityBasicPrevious.Check();

    [Rpc(CallLocal = true)]
    protected void SetAbilityBasicNext() => Set(AbilityBasicNext);
    public override bool GetAbilityBasicNext() => AbilityBasicNext.Check();

    [Rpc(CallLocal = true)]
    protected void SetAbilityComplex1() => Set(AbilityComplex1);
    public override bool GetAbilityComplex1() => AbilityComplex1.Check();

    [Rpc(CallLocal = true)]
    protected void SetAbilityComplex2() => Set(AbilityComplex2);
    public override bool GetAbilityComplex2() => AbilityComplex2.Check();

    [Rpc(CallLocal = true)]
    protected void SetAbilityComplex3() => Set(AbilityComplex3);
    public override bool GetAbilityComplex3() => AbilityComplex3.Check();

    [Rpc(CallLocal = true)]
    protected void SetAbilityGodlike() => Set(AbilityGodlike);
    public override bool GetAbilityGodlike() => AbilityGodlike.Check();


    [Rpc(CallLocal = true)]
    protected void SetAbilityIndex(int idx) => AbilityIndex = idx;
    public override int GetAbilityBasicIndex() => AbilityIndex;
}
