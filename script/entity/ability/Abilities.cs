using Godot;
using System;
using System.Linq;

public partial class Abilities : Node
{
    [ExportSubgroup("Settings")]
    [Export] public int BasicIndex { get; set; } = 0;
    

    // TODO: possibly cache node references?
    public Ability[] Basic => BasicParent.GetChildren().Select(ch => (Ability)ch).ToArray();
    public Ability Complex1 => ComplexParent1.GetChildOrNull<Ability>(0);
    public Ability Complex2 => ComplexParent2.GetChildOrNull<Ability>(0);
    public Ability Complex3 => ComplexParent3.GetChildOrNull<Ability>(0);
    public Ability Godlike => GodlikeParent.GetChildOrNull<Ability>(0);

    public Ability[] Complex => [Complex1, Complex2, Complex3];
    public Ability BasicSelected => Basic.ElementAtOrDefault(BasicIndex);

    public Ability[] All => BasicParent.GetChildren().Select(ch => (Ability)ch)
        .Concat([Complex1, Complex2, Complex3, Godlike])
        .Where(a => a != null)
        .ToArray();

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

    public void OnCastCancelInput(Entity owner)
    {
        foreach(var ability in All)
            ability.OnCastCancelInput(owner);
    }
}
