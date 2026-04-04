using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.power;

public class Mordekaiser_spreadpower : PowerModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5m, ValueProp.Move)];
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if(cardSource is { IsUpgraded: true })
        {
            DynamicVars.Damage.UpgradeValueBy(5m);
        }
        return Task.CompletedTask;
    }
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        if (Owner.CombatState == null) return;
        await CreatureCmd.Damage(choiceContext, Owner.CombatState.Enemies, DynamicVars.Damage, Owner);
        Flash();
        await PowerCmd.ModifyAmount(this, -1, Owner, null);
    }
    
}