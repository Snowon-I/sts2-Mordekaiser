using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.scripts;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.power;

public class Mordekaiser_deathdominionpower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        await MordekaiserCardUtils.TriggerMordekaiserCardQuiesce(
            choiceContext,
            player,
            Amount,
            PileType.Draw.GetPile(player), 
            cards => cards.Where(c => c.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce)).Take(Amount)
        );
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (player != Owner.Player) return;
        await MordekaiserCardUtils.TriggerMordekaiserCardQuiesce(
            choiceContext,
            player,
            Amount,
            PileType.Draw.GetPile(player), 
            cards => cards.Where(c => c.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce)).Take(Amount)
        );
    }
}