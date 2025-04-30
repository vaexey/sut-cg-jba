using Godot;
using System;

public partial class AbilityBar : Control
{
    [ExportSubgroup("Nodes")]
    [Export] public Node2D EntityContainer { get; set; }
    // protected IEntityContainer _EntityContainer => (IEntityContainer)EntityContainer;

    [ExportSubgroup("UI Nodes")]
    [Export] public AbilityIcon Basic1 { get; set; }
    [Export] public AbilityIcon Godlike { get; set; }

    public override void _Process(double delta)
    {
        // TODO: Probably not efficient
        Basic1.EntityContainer = EntityContainer;
        Godlike.EntityContainer = EntityContainer;

        var basic = ((IEntityContainer)EntityContainer).Entity.Abilities.Basic;
        Basic1.Ability = basic[0];
        Godlike.Ability = ((IEntityContainer)EntityContainer).Entity.Abilities.Godlike;
    }
}
