
/*

using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Rooms;
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
}

*/