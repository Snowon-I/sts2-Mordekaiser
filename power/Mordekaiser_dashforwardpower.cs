using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Mordekaiser.cards;

namespace Mordekaiser.power;

public class Mordekaiser_dashforwardpower : TemporaryStrengthPower
{
    public override AbstractModel OriginModel => ModelDb.Card<Mordekaiser_com_dashforward>();
}
