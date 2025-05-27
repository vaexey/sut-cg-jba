using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[Tool]
public partial class AbilityIcon : Control
{
    [ExportSubgroup("Settings")]
    [Export] public bool Round { get; set; } = false;
    [Export] public bool Highlight { get; set; } = false;

    [ExportSubgroup("Nodes")]
    [Export] public Ability Ability { get; set; }
    [Export] public Node2D EntityContainer { get; set; }

    [Export] public CanvasGroup TintCanvas { get; set; }
    [Export] public Sprite2D BackgroundSprite { get; set; }
    [Export] public Sprite2D IconSprite { get; set; }
    [Export] public Label OverlayLabel { get; set; }

    [ExportSubgroup("Resources")]
    [Export] public Texture2D DefaultAtlas { get; set; }
    [Export] public Texture2D HighlightAtlas { get; set; }
    [Export] public Texture2D RoundAtlas { get; set; }
    [Export] public Texture2D RoundHighlightAtlas { get; set; }

    // protected IEntityContainer _EntityContainer => (IEntityContainer)EntityContainer;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        var atlas = (AtlasTexture)BackgroundSprite.Texture;
        
        if(!Round)
        {
            atlas.Atlas = !Highlight ? DefaultAtlas : HighlightAtlas;
        } else {
            atlas.Atlas = !Highlight ? RoundAtlas : RoundHighlightAtlas;
        }
        
        if(Engine.IsEditorHint()) return;

        var shader = (ShaderMaterial)TintCanvas.Material;

        if(Ability == null)
        {
            shader.SetShaderParameter("tint_enable", true);
            OverlayLabel.Text = "";
            return;
        }

        var trial = Ability.CanUse((IEntityContainer)EntityContainer);
        var canUse = trial == AbilityUsageTrialResult.OK;

        shader.SetShaderParameter("tint_enable", !canUse);

        // TODO: Probably not efficient
        IconSprite.Texture = Ability.IconTexture;

        OverlayLabel.Text = (trial == AbilityUsageTrialResult.OnCooldown) ?
            Ability.CooldownLeft.ToString("N1") : 
            "";

        TooltipText = WordWrap(Ability.ShortDescription, 40).ToArray().Join("\n");
    }
    public static List<string> WordWrap( string text, int maxLineLength )
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