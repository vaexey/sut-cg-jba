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

	public Node2D Parent2D => GetParent<Node2D>();
	#endregion

	#region Realtime attributes
	public AttributeValue Beverage { get; protected set; }
	public AttributeValue Stamina { get; protected set; }
	public AttributeValue Inspiration { get; protected set; }

	public bool IsAlive { get; set; } = true;
	public AttributeSemaphore IsCrippledHorizontally { get; set; } = 0;
	public AttributeSemaphore IsCrippledVertically { get; set; } = 0;
	public AttributeSemaphore IsSilenced { get; set; } = 0;
	public Vector2 PointingAt { get; set; } = Vector2.Zero;

	public bool IsCasting => Abilities.All.Where(a => a.IsCasting).Any();

	#endregion


	[Signal]
	public delegate void OnDamagedEventHandler();

	// [Export]
	// public WorldType World { get; set; }
	public WorldType World => WorldType.FindFor(this);

	[ExportSubgroup("Multiplayer sync")]
	[Export] private double MPS_Beverage { get => Beverage.Percentage; set => Beverage.Percentage = value; } 
	[Export] private double MPS_Stamina { get => Stamina.Percentage; set => Stamina.Percentage = value; } 
	[Export] private double MPS_Inspiration { get => Inspiration.Percentage; set => Inspiration.Percentage = value; } 

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

	// public override void _Ready()
	// {
	// 	World = WorldType.FindFor(this);
	// }

	public override void _PhysicsProcess(double delta)
	{
		// Stamina.Value = Mathf.MoveToward(
		// 	Stamina.Value, 
		// 	Stamina.Max, 
		// 	PassiveAttributes.StaminaRegen * delta
		// 	);

		if (Beverage.Percentage <= 0)
		{
			// IsAlive = false;
			Kill();
		}

		if (IsAlive)
		{
			Beverage.Regen = PassiveAttributes.BeverageRegen;
			Beverage.Process(delta);

			Stamina.Regen = PassiveAttributes.StaminaRegen;
			Stamina.Process(delta);

			Inspiration.Regen = PassiveAttributes.InspirationRegen;
			Inspiration.Process(delta);
		}
	}

	public void Kill()
	{
		IsAlive = false;

		CC.Cleanse();
		IsSilenced = true;
		IsCrippledHorizontally = true;
		IsCrippledVertically = true;
	}

	public bool CanJump()
	{
		return Stamina.Value > PassiveAttributes.StaminaMinJump
			&& !IsCrippledVertically;
	}

	public void DidJump()
	{
		// Stamina.Value -= PassiveAttributes.StaminaUsageJump;
		ConsumeStamina(
			PassiveAttributes.StaminaUsageJump *
			PassiveAttributes.StaminaUsageModifier
		);
	}

	public void ApplyDamage(Damage dmg)
	{
		// GD.Print($"Applied {dmg.FlatValue} damage to {Name}");

		// GD.Print($"BEFORE: {Beverage.Value}");
		// Beverage.Value -= dmg.FlatValue;
		// GD.Print($"AFTER: {Beverage.Value}");


		double flat = dmg.FlatValue + dmg.PercentageValue * Beverage.Max;
		GD.Print($"Damage to {Name}: {dmg.FlatValue}(+{dmg.PercentageValue}%) == {flat}");

		if (dmg.Flags.HasFlag(DamageFlags.SourcePhysical))
			flat = PassiveAttributes.PhysicalDamageProcess(flat);
		if (dmg.Flags.HasFlag(DamageFlags.SourceInspired))
			flat = PassiveAttributes.InspiredDamageProcess(flat);

		GD.Print($"After reductions: {flat}");
		GD.Print($"BEFORE: {Beverage.Value}");
		Beverage.Value -= flat;
		GD.Print($"AFTER: {Beverage.Value}");

		EmitSignal(SignalName.OnDamaged);
	}

	public void ConsumeStamina(double value)
	{
		Stamina.Value -= value;
	}

	public void ConsumeInspiration(double value)
	{
		Inspiration.Value -= value;
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
