using Godot;
using System;

public partial class MussDasSeinAbility : Ability
{

	[Signal]
	public delegate void OnFailedEventHandler();
	[Signal]
	public delegate void OnReflectedEventHandler();

    public override void Cast(Entity owner)
    {
        var muss = CrowdControlLibrary.MussDasSein.Make();

        owner.CC.AddEffect(muss, 0.75);
    }

    public void OnReflectedSignal()
    {
        EmitSignal(SignalName.OnReflected);
    }

    public void OnFailedSignal()
    {
        EmitSignal(SignalName.OnFailed);
    }
}
