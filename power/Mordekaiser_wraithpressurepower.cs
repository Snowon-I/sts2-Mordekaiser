using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Mordekaiser.cards;

namespace Mordekaiser.power;

public class Mordekaiser_wraithpressurepower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Mordekaiser_unc_wraithpressure>();

    protected override bool IsPositive => false;
}