using Godot;
using System;
using System.Linq;

public partial class AbilityBar : Control
{
    [ExportSubgroup("Nodes")]
    [Export] public Entity Entity { get; set; }

    [ExportSubgroup("UI Nodes")]
    [Export] public Node BasicList { get; set; }
    [Export] public AbilityIcon Complex1 { get; set; }
    [Export] public AbilityIcon Complex2 { get; set; }
    [Export] public AbilityIcon Complex3 { get; set; }
    [Export] public AbilityIcon Godlike { get; set; }

    public override void _Process(double delta)
    {
        // TODO: Probably not efficient
        Complex1.Entity = Entity;
        Complex2.Entity = Entity;
        Complex3.Entity = Entity;
        Godlike.Entity = Entity;

        var abilities = Entity.Abilities;
        
        for(int i = 0; i < 10; i++)
        {
            var icon = BasicList.GetChild<AbilityIcon>(i);
            var basic = abilities.Basic.ElementAtOrDefault(i);

            icon.Entity = Entity;
            icon.Ability = basic;
            icon.Highlight = i == abilities.BasicIndex;
        }

        Complex1.Ability = abilities.Complex1;
        Complex2.Ability = abilities.Complex2;
        Complex3.Ability = abilities.Complex3;
        Godlike.Ability = abilities.Godlike;
    }
}
