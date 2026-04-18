using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Mordekaiser.power;

public class Mordekaiser_soulreavepower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (Owner.CombatState is null) return;
        if (amount >= 0) return;
        if (applier == null) return;
        if (power.Owner.IsPlayer) return;
        if (power.Id != ModelDb.Power<StrengthPower>().Id) return;
        await PowerCmd.Apply<StrengthPower>(Owner, Amount, Owner, null);
    }
}