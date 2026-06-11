
/*

using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Rooms;

namespace ActsFromThePast.Patches.Debug;

public class DebugPatches
{
    [HarmonyPatch(typeof(UnknownMapPointOdds), nameof(UnknownMapPointOdds.Roll))]
    public static class ForceShopPatch
    {
        public static bool Prefix(ref RoomType __result)
        {
            __result = RoomType.Shop;
            return false;
        }
    }
    
    
        [HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.Add), new[] { typeof(Creature) })]
    public static class CreatureAddPositionLogger
    {
        public static void Postfix(Creature creature)
        {
            var node = NCombatRoom.Instance?.GetCreatureNode(creature);
            if (node == null) return;
            Log.Info($"[CreatureAdd] {creature.Monster?.GetType().Name} at {node.GlobalPosition}");
        }
    }
    
    [HarmonyPatch(typeof(CombatManager), nameof(CombatManager.AfterCreatureAdded))]
    public static class AfterCreatureAddedPositionLogger
    {
        public static void Postfix(Creature creature)
        {
            var node = NCombatRoom.Instance?.GetCreatureNode(creature);
            if (node == null) return;
            Log.Info($"[AfterCreatureAdded] {creature.Monster?.GetType().Name} at {node.GlobalPosition}");
        }
    }
    
[HarmonyPatch(typeof(NMainMenu), "_Ready")]
public static class GreenScreenButtonPatch
{
    static void Postfix(NMainMenu __instance)
    {
        foreach (Node child in __instance.GetChildren())
        {
            if (child is Control ctrl)
                ctrl.Visible = false;
        }

        var greenBg = new ColorRect();
        greenBg.Color = new Color(0, 1, 0);
        greenBg.SetAnchorsPreset(Control.LayoutPreset.FullRect);
        __instance.AddChild(greenBg);

        var buttonScene = GD.Load<PackedScene>(
            "res://scenes/ui/abandon_run_no_button.tscn"
        );
        var button = buttonScene.Instantiate<Control>();

        var label = button.GetNode<Label>("Visuals/Label");
        label.Text = "Exordium";

        // Set outline to black by default
        label.AddThemeColorOverride("font_outline_color", Colors.Black);

        var image = button.GetNode<TextureRect>("Visuals/Image");
        var material = (ShaderMaterial)image.Material;

        button.SetAnchorsPreset(Control.LayoutPreset.Center);
        button.GrowHorizontal = Control.GrowDirection.Both;
        button.GrowVertical = Control.GrowDirection.Both;
        button.Scale = new Vector2(3.0f, 3.0f);
        greenBg.AddChild(button);
        button.CallDeferred("set_pivot_offset", button.Size / 2f);

        float rightX = __instance.GetViewportRect().Size.X - 320;
        float y = 20f;

        // --- Background color picker (top-left) ---
        var bgPicker = new ColorPicker();
        bgPicker.Color = greenBg.Color;
        bgPicker.Position = new Vector2(20, 20);
        bgPicker.ColorChanged += (color) => greenBg.Color = color;
        __instance.AddChild(bgPicker);

        // --- Text input ---
        var textLabel = new Label();
        textLabel.Text = "Button Text";
        textLabel.Position = new Vector2(rightX, y);
        __instance.AddChild(textLabel);
        y += 30;

        var textInput = new LineEdit();
        textInput.Text = "Exordium";
        textInput.CustomMinimumSize = new Vector2(280, 36);
        textInput.Position = new Vector2(rightX, y);
        textInput.TextChanged += (newText) => label.Text = newText;
        __instance.AddChild(textInput);
        y += 56;

        // --- Font color ---
        var fontColorLabel = new Label();
        fontColorLabel.Text = "Font Color";
        fontColorLabel.Position = new Vector2(rightX, y);
        __instance.AddChild(fontColorLabel);
        y += 30;

        var fontColorPicker = new ColorPickerButton();
        fontColorPicker.Color = label.GetThemeColor("font_color");
        fontColorPicker.CustomMinimumSize = new Vector2(280, 36);
        fontColorPicker.Position = new Vector2(rightX, y);
        fontColorPicker.ColorChanged += (color) =>
            label.AddThemeColorOverride("font_color", color);
        __instance.AddChild(fontColorPicker);
        y += 56;

        // --- Outline color ---
        var outlineColorLabel = new Label();
        outlineColorLabel.Text = "Outline Color";
        outlineColorLabel.Position = new Vector2(rightX, y);
        __instance.AddChild(outlineColorLabel);
        y += 30;

        var outlineColorPicker = new ColorPickerButton();
        outlineColorPicker.Color = Colors.Black;
        outlineColorPicker.CustomMinimumSize = new Vector2(280, 36);
        outlineColorPicker.Position = new Vector2(rightX, y);
        outlineColorPicker.ColorChanged += (color) =>
            label.AddThemeColorOverride("font_outline_color", color);
        __instance.AddChild(outlineColorPicker);
        y += 56;
        
// --- Font size ---
        var sizeLabel = new Label();
        sizeLabel.Text = "Font Size";
        sizeLabel.Position = new Vector2(rightX, y);
        __instance.AddChild(sizeLabel);
        y += 30;

        var sizeSpinBox = new SpinBox();
        sizeSpinBox.MinValue = 8;
        sizeSpinBox.MaxValue = 72;
        sizeSpinBox.Step = 1;
        sizeSpinBox.Value = 28;
        sizeSpinBox.CustomMinimumSize = new Vector2(280, 36);
        sizeSpinBox.Position = new Vector2(rightX, y);
        sizeSpinBox.ValueChanged += (val) =>
            label.AddThemeFontSizeOverride("font_size", (int)val);
        __instance.AddChild(sizeSpinBox);
        y += 56;
        // --- Hue slider ---
        var hueLabel = new Label();
        hueLabel.Text = "Hue";
        hueLabel.Position = new Vector2(rightX, y);
        __instance.AddChild(hueLabel);
        y += 30;

        var hueSlider = new HSlider();
        hueSlider.MinValue = 0.0;
        hueSlider.MaxValue = 1.0;
        hueSlider.Step = 0.01;
        hueSlider.Value = 1.0;
        hueSlider.CustomMinimumSize = new Vector2(280, 30);
        hueSlider.Position = new Vector2(rightX, y);
        __instance.AddChild(hueSlider);
        y += 50;

        // --- Paleness slider ---
        var paleLabel = new Label();
        paleLabel.Text = "Paleness";
        paleLabel.Position = new Vector2(rightX, y);
        __instance.AddChild(paleLabel);
        y += 30;

        var paleSlider = new HSlider();
        paleSlider.MinValue = 0.0;
        paleSlider.MaxValue = 1.0;
        paleSlider.Step = 0.01;
        paleSlider.Value = 0.75;
        paleSlider.CustomMinimumSize = new Vector2(280, 30);
        paleSlider.Position = new Vector2(rightX, y);
        __instance.AddChild(paleSlider);

        hueSlider.ValueChanged += (_) => UpdateButton(material, hueSlider, paleSlider);
        paleSlider.ValueChanged += (_) => UpdateButton(material, hueSlider, paleSlider);
    }

    private static void UpdateButton(
        ShaderMaterial material,
        HSlider hueSlider,
        HSlider paleSlider)
    {
        material.SetShaderParameter("h", (float)hueSlider.Value);
        material.SetShaderParameter("s", 1.0f - (float)paleSlider.Value);
        material.SetShaderParameter("v", 1.2f);
    }
}
}

*/