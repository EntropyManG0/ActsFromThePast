using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace ActsFromThePast.Relics;

[Pool(typeof(EventRelicPool))]
public sealed class NlothsHungryFace : CustomRelicModel
{
    private int _treasureRoomsEntered;
    
    public override RelicRarity Rarity => RelicRarity.Event;
    public override bool IsUsedUp => _treasureRoomsEntered >= 1;
    
    [SavedProperty]
    public int TreasureRoomsEntered
    {
        get => _treasureRoomsEntered;
        set
        {
            AssertMutable();
            _treasureRoomsEntered = value;
            if (IsUsedUp)
                Status = RelicStatus.Disabled;
        }
    }
    
    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is TreasureRoom && _treasureRoomsEntered == 0)
        {
            ++TreasureRoomsEntered;
            Flash();
        }
        return Task.CompletedTask;
    }
    
    public override bool ShouldGenerateTreasure(Player player)
    {
        if (player != Owner || TreasureRoomsEntered > 1)
            return true;
        
        var silverCrucible = Owner.Relics.OfType<SilverCrucible>().FirstOrDefault();
        if (silverCrucible != null && !silverCrucible.ShouldGenerateTreasure(player))
            return true;
        
        return false;
    }
}