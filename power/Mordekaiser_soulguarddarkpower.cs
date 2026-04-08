using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.scripts;

namespace Mordekaiser.power;

public class Mordekaiser_soulguarddarkpower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side) return;
        if (CombatManager.Instance.History.MordekaiserQuiesceEvents().Any(history => history.HappenedThisTurn(CombatState) && history.Card.Owner == Owner.Player))
        {
            await CreatureCmd.GainBlock(Owner, Amount,ValueProp.Unpowered,null);
        }
    }
}