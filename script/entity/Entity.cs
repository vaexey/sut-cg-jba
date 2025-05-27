using Godot;
using System;
using System.Linq;
using WorldType = World;

public partial class Entity : Node
{
	#region Essential nodes
	[Export] public PassiveAttributes PassiveAttributes { get; set; }

	[Export] public CrowdControl CC { get; set; }

	[Export] public Abilities Abilities { get; set; }
	#endregion

	#region Realtime attributes
	public AttributeValue Beverage { get; protected set; }
	public AttributeValue Stamina { get; protected set; }
	public AttributeValue Inspiration { get; protected set; }

	public bool IsAlive { get; set; } = true;
	public AttributeSemaphore IsCrippledHorizontally { get; set; } = 0;
	public AttributeSemaphore IsCrippledVertically { get; set; } = 0;
	public AttributeSemaphore IsSilenced { get; set; } = 0;

	public bool IsCasting => Abilities.All.Where(a => a.IsCasting).Any();

	#endregion

	[Export]
	public WorldType World { get; set; }

	public Entity()
	{
		Beverage = new(
			max: () =>
				PassiveAttributes.BoozeToleranceIndex *
				PassiveAttributes.BeverageMaxFromBTI
		);
		Stamina = new(
			max: 1.0
		);
		Inspiration = new(
			max: () =>
				PassiveAttributes.OpenMindedness *
				PassiveAttributes.InspirationMaxFromOM
		);
	}

    public override void _Ready()
    {
		World = WorldType.FindFor(this);
    }

    public override void _PhysicsProcess(double delta)
    {
		// Stamina.Value = Mathf.MoveToward(
		// 	Stamina.Value, 
		// 	Stamina.Max, 
		// 	PassiveAttributes.StaminaRegen * delta
		// 	);

		if(Beverage.Percentage <= 0)
		{
			IsAlive = false;
		}

		if(IsAlive)
		{
			Stamina.Regen = PassiveAttributes.StaminaRegen;
			Stamina.Process(delta);
		}
    }

	public bool CanJump()
	{
		return Stamina.Value > PassiveAttributes.StaminaMinJump;
	}

	public void DidJump()
	{
		Stamina.Value -= PassiveAttributes.StaminaUsageJump;
	}

	public void ApplyDamage(Damage dmg)
	{
		GD.Print($"Applied {dmg.FlatValue} damage to {Name}");

		GD.Print($"BEFORE: {Beverage.Value}");
		Beverage.Value -= dmg.FlatValue;
		GD.Print($"AFTER: {Beverage.Value}");
	}

	// #region Beverage
	// public double BeveragePercentage { get; set; } = 1.0;
	// public double BeverageMax {
	// 	get {
	// 		return 
	// 			PassiveAttributes.BoozeToleranceIndex * 
	// 			PassiveAttributes.BTI_BEVERAGEMAX_MODIFIER;
	// 	}
	// }
	// public double BeverageValue {
	// 	get {
	// 		return BeveragePercentage * BeverageMax;
	// 	}
	// 	set {
	// 		BeveragePercentage = value / BeverageMax;
	// 	}
	// }
	// #endregion
}
