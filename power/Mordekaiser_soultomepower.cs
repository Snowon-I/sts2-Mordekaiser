using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.relics;

namespace Mordekaiser.power;

public class Mordekaiser_soultomepower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != Owner) return Task.CompletedTask;
        if (!result.WasTargetKilled) return Task.CompletedTask;
        var relic = Owner.Player?.GetRelic<Mordekaiser_soulcrown>();
        var relicorbas = Owner.Player?.GetRelic<Mordekaiser_soulcrown_orobas>();
        if (relic != null )
            relic.MonsterSouls++;
        if (relicorbas != null )
            relicorbas.MonsterSouls++;
        return Task.CompletedTask;
    }
}