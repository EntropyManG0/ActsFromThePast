using ActsFromThePast.Acts.TheCity.Events;
using HarmonyLib;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;

namespace ActsFromThePast.Patches.RoomEvents;

public class MaskedBanditsPatches
{
    private static readonly HashSet<Type> BanditEncounters = new()
    {
        typeof(RedMaskBanditsEvent)
    };

    [HarmonyPatch(typeof(RewardsSet), nameof(RewardsSet.WithRewardsFromRoom))]
    public class RewardsPatch
    {
        public static void Postfix(RewardsSet __result, AbstractRoom room)
        {
            if (room is not CombatRoom combatRoom)
                return;
            if (!BanditEncounters.Contains(
                    combatRoom.Encounter.GetType()))
                return;

            var extraRewards = combatRoom.ExtraRewards.Values
                .SelectMany(list => list)
                .ToHashSet();

            __result.Rewards.RemoveAll(r =>
                !extraRewards.Contains(r) &&
                r is GoldReward);
        }
    }

    [HarmonyPatch(typeof(NMapScreen), nameof(NMapScreen.Close))]
    public class MapClosePatch
    {
        public static void Postfix(NMapScreen __instance)
        {
            string descKey = null;
            if (MaskedBandits.WaitingForMapEasterEgg)
            {
                MaskedBandits.WaitingForMapEasterEgg = false;
                descKey = "ACTSFROMTHEPAST-MASKED_BANDITS.pages.PAID_4.description";
            }
            else if (MaskedBandits.WaitingForBrandishEasterEgg)
            {
                MaskedBandits.WaitingForBrandishEasterEgg = false;
                descKey = "ACTSFROMTHEPAST-MASKED_BANDITS.pages.BRANDISH_4.description";
            }
            if (descKey == null)
                return;
            if (NEventRoom.Instance?.Layout is not { } layout)
                return;
            var descLabel = layout.GetNodeOrNull<MegaRichTextLabel>("EventDescription");
            if (descLabel == null)
                return;
            var locString = new LocString("events", descKey);
            descLabel.Text = locString.GetFormattedText();
        }
    }
}