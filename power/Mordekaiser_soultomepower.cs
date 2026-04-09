using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.relics;
using Mordekaiser.scripts;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.power;

public class Mordekaiser_soultomepower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != Owner) return Task.CompletedTask;
        if (!result.WasTargetKilled) return Task.CompletedTask;
        var relic = Owner.Player?.GetRelic<Mordekaiser_relic>();
        if (relic == null ) return Task.CompletedTask;
        relic.MonsterSouls++;
        return Task.CompletedTask;
    }
}