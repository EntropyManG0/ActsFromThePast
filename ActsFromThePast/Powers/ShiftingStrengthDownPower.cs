using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ActsFromThePast.Powers;

public class ShiftingStrengthDownPower : TemporaryStrengthPower, ICustomPower
{
    public override AbstractModel OriginModel => ModelDb.Power<ShiftingPower>();
    protected override bool IsPositive => false;

    public override LocString Title =>
        ModelDb.Power<ShiftingPower>().Title;

    protected override IEnumerable<IHoverTip> ExtraHoverTips
    {
        get
        {
            return new IHoverTip[]
            {
                HoverTipFactory.FromPower<ShiftingPower>(),
                HoverTipFactory.FromPower<StrengthPower>()
            };
        }
    }
}