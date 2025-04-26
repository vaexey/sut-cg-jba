using Godot;
using System;

public partial class Entity : Node
{
	[Export]
	public PassiveAttributes PassiveAttributes { get; set; }

	public AttributeValue Beverage { get; protected set; }
	public AttributeValue Stamina { get; protected set; }
	public AttributeValue Inspiration { get; protected set; }

	public Entity()
	{
		Beverage = new(
			max: () =>
				PassiveAttributes.BoozeToleranceIndex *
				PassiveAttributes.BTI_BEVERAGEMAX_MODIFIER
		);
		Stamina = new(
			max: 1.0
		);
		Inspiration = new(
			max: () =>
				PassiveAttributes.OpenMindedness *
				PassiveAttributes.OM_INSPIRATION_MODIFIER
		);
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
