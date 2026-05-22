using ActsFromThePast.Acts;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace ActsFromThePast;

public sealed class LotsOfSlimesNormal : CustomEncounterModel
{
    public LotsOfSlimesNormal() : base(RoomType.Monster)
    {
    }
    
    public override bool IsValidForAct(ActModel act) => act is ExordiumAct;
    public override IEnumerable<EncounterTag> Tags => [EncounterTag.Slimes];
    public override bool IsWeak => false;
    public override bool HasScene => true;
    public override IReadOnlyList<string> Slots => new[] { "first", "second", "third", "fourth", "fifth" };
    
    private static MonsterModel[] SmallSlimes => new MonsterModel[]
    {
        ModelDb.Monster<SpikeSlimeSmall>(),
        ModelDb.Monster<AcidSlimeSmall>()
    };
    
    public override IEnumerable<MonsterModel> AllPossibleMonsters => SmallSlimes;
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        var pool = new List<Func<MonsterModel>>
        {
            () => ModelDb.Monster<SpikeSlimeSmall>().ToMutable(),
            () => ModelDb.Monster<SpikeSlimeSmall>().ToMutable(),
            () => ModelDb.Monster<SpikeSlimeSmall>().ToMutable(),
            () => ModelDb.Monster<AcidSlimeSmall>().ToMutable(),
            () => ModelDb.Monster<AcidSlimeSmall>().ToMutable()
        };
    
        var result = new List<(MonsterModel, string?)>();
        for (int i = 0; i < Slots.Count; i++)
        {
            var index = Rng.NextInt(pool.Count);
            result.Add((pool[index](), Slots[i]));
            pool.RemoveAt(index);
        }
    
        return result;
    }
}