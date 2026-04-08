using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.power;

public class Mordekaiser_emberbladeinitiatepower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        var _card= await MordekaiserCardUtils.DrawMordekaiserTypeCard(
            choiceContext,
            player,
            Amount,
            PileType.Draw.GetPile(player), PileType.Hand.GetPile(player),
            cards => cards.Take(Amount),
            true
        );
        foreach (var card in _card.Where(card => !card.Keywords.Contains(CardKeyword.Exhaust)))
            CardCmd.ApplyKeyword(card, CardKeyword.Exhaust);
    }
    
}