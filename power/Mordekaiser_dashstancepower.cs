using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mordekaiser.power;

public class Mordekaiser_dashstancepower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card.Owner.Creature != Owner || card.Type != CardType.Attack)
        {
            modifiedCost = originalCost;
            return false;
        }
        modifiedCost = 0;
        return true;
    }

    public override bool TryModifyStarCost(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card.Owner.Creature != Owner || card.Type != CardType.Attack)
        {
            modifiedCost = originalCost;
            return false;
        }
        modifiedCost = 0;
        return true;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature == Owner && cardPlay is { IsAutoPlay: false, IsLastInSeries: true, Card.Type: CardType.Attack } )
            await PowerCmd.ModifyAmount(this,-1,Owner,null);
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side) return;
        await PowerCmd.Remove<Mordekaiser_dashstancepower>(Owner);
    }
    
}