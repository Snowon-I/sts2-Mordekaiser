using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.relics;

namespace Mordekaiser.Mordekaiserpools;

public class Mordekaiserrelicpool : RelicPoolModel
{
    public override string EnergyColorName => "mordekaiser";

    public override Color LabOutlineColor => StsColors.red;

    protected override IEnumerable<RelicModel> GenerateAllRelics() => [ 
        ModelDb.Relic<Mordekaiser_relic>()
    ];

}