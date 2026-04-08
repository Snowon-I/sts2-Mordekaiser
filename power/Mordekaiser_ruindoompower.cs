using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.power;

public class Mordekaiser_ruindoompower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool IsInstanced => true;

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner == Owner.Player && Owner.CombatState != null )
        {
            var target = Owner.CombatState.Enemies.TakeRandom(1,Owner.Player.RunState.Rng.CombatCardSelection);
            await CreatureCmd.Damage(choiceContext,target,Amount,ValueProp.Unpowered,Owner);
        }
    }
    
}