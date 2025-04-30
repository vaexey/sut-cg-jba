using Godot;
using System;
using System.Linq;

public partial class Abilities : Node
{
    // TODO: possibly cache node references?
    public Ability[] Basic => BasicParent.GetChildren().Select(ch => (Ability)ch).ToArray();
    public Ability Complex1 => (Ability)ComplexParent1.GetChild(0);
    public Ability Complex2 => (Ability)ComplexParent2.GetChild(0);
    public Ability Complex3 => (Ability)ComplexParent3.GetChild(0);
    public Ability Godlike => (Ability)GodlikeParent.GetChild(0);

    public Ability[] Complex => [Complex1, Complex2, Complex3];

    protected Node BasicParent;
    protected Node ComplexParent1;
    protected Node ComplexParent2;
    protected Node ComplexParent3;
    protected Node GodlikeParent;

    public override void _Ready()
    {
        BasicParent = GetNode("Basic");
        ComplexParent1 = GetNode("Complex/Slot 1");
        ComplexParent2 = GetNode("Complex/Slot 2");
        ComplexParent3 = GetNode("Complex/Slot 3");
        GodlikeParent = GetNode("Godlike");
    }
}
