using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;

namespace ActsFromThePast;

public sealed class RedMaskBanditsEvent : CustomEncounterModel
{
    public RedMaskBanditsEvent() : base(RoomType.Monster)
    {
    }

    public override bool IsValidForAct(ActModel act) => false;
    public override IEnumerable<EncounterTag> Tags => Array.Empty<EncounterTag>();
    public override bool IsWeak => false;
    public override bool HasScene => true;
    public override IReadOnlyList<string> Slots => new[] { "pointy", "romeo", "bear" };

    public override IEnumerable<MonsterModel> AllPossibleMonsters
    {
        get
        {
            yield return ModelDb.Monster<Pointy>();
            yield return ModelDb.Monster<Romeo>();
            yield return ModelDb.Monster<Bear>();
        }
    }
    
    protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
    {
        return new List<(MonsterModel, string?)>
        {
            (ModelDb.Monster<Pointy>().ToMutable(), "pointy"),
            (ModelDb.Monster<Romeo>().ToMutable(), "romeo"),
            (ModelDb.Monster<Bear>().ToMutable(), "bear")
        };
    }
}