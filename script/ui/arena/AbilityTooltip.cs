using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AbilityTooltip : Control
{
    [ExportSubgroup("Settings")]
    [Export(PropertyHint.ColorNoAlpha)]
    public Color CostColorPhysical { get; set; } = Color.Color8(128, 255, 128, 255);

    [Export(PropertyHint.ColorNoAlpha)]
    public Color CostColorInspired { get; set; } = Color.Color8(128, 128, 255, 255);
    [Export]
    public int WordWrapMaxLength { get; set; } = 40;

    [ExportSubgroup("Nodes")]
    [Export] Label DisplayName { get; set; }
    [Export] Label Cost { get; set; }
    [Export] Label Description { get; set; }

    [Export] Control ProjectileAppendix { get; set; }
    [Export] TextureRect ProjectileIcon { get; set; }
    [Export] Label ProjectileName { get; set; }
    [Export] Label ProjectileDesc { get; set; }

    protected Ability TempAbility;

    public void MakeTooltip(string path)
    {
        TempAbility = (Ability)ResourceLoader.Load<PackedScene>(path).Instantiate();

        TempAbility.Ready += () => TempAbility.Sync.QueueFree();

        AddChild(TempAbility);
        MakeTooltip(TempAbility);
    }

    public void MakeTooltip(Ability ability)
    {
        DisplayName.Text = ability.DisplayName;

        List<string> cost = new();
        string unit = "";

        if (ability.CategoryType == AbilityCategory.Physical)
        {
            if (ability.UseCostFlat > 0 | ability.UseCostPercentageMax > 0)
                cost.Add($"{Math.Round((ability.UseCostFlat + ability.UseCostPercentageMax) * 1000) / 10}% max");
            if (ability.UseCostPercentage > 0)
                cost.Add($"{Math.Round(ability.UseCostPercentage * 1000) / 10}% of current");

            unit = " stamina";
        }
        else
        {
            if (ability.UseCostFlat > 0)
                cost.Add($"{Math.Round(ability.UseCostFlat * 10) / 10}");
            if (ability.UseCostPercentageMax > 0)
                cost.Add($"{Math.Round(ability.UseCostPercentageMax * 1000) / 10}% max");
            if (ability.UseCostPercentage > 0)
                cost.Add($"{Math.Round(ability.UseCostPercentage * 1000) / 10}% of current");

            unit = " inspiration";
        }

        if (cost.Count() == 0)
            unit = "Free";

        Cost.Text = string.Join(" + ", cost) + unit;

        Cost.LabelSettings.FontColor =
            ability.CategoryType == AbilityCategory.Inspired
                ? CostColorInspired
                : CostColorPhysical;

        Description.Text = ability.ShortDescription;

        Description.Text = WordWrap(ability.ShortDescription, WordWrapMaxLength)
            .ToArray()
            .Join("\n");

        ProjectileAppendix.Visible = false;
    }

    protected static List<string> WordWrap( string text, int maxLineLength )
    {
        var list = new List<string>();

        int currentIndex;
        var lastWrap = 0;
        var whitespace = new[] { ' ', '\r', '\n', '\t' };
        do
        {
            currentIndex = lastWrap + maxLineLength > text.Length ? text.Length : (text.LastIndexOfAny( new[] { ' ', ',', '.', '?', '!', ':', ';', '-', '\n', '\r', '\t' }, Math.Min( text.Length - 1, lastWrap + maxLineLength)  ) + 1);
            if( currentIndex <= lastWrap )
                currentIndex = Math.Min( lastWrap + maxLineLength, text.Length );
            list.Add( text.Substring( lastWrap, currentIndex - lastWrap ).Trim( whitespace ) );
            lastWrap = currentIndex;
        } while( currentIndex < text.Length );

        return list;
    }

}
