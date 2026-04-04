using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;

namespace Mordekaiser.Mordekaiserpools;

public sealed class Mordekaiserpotionpool : PotionPoolModel
{
    public override string EnergyColorName => "ironclad";

    public override Color LabOutlineColor => StsColors.red;

    protected override IEnumerable<PotionModel> GenerateAllPotions()
    {
        return Ironclad4Epoch.Potions;
    }

}
