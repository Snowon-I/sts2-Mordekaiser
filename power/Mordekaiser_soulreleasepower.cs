using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.scripts;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.power;

public class Mordekaiser_soulreleasepower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner == Owner.Player && card.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce))
        {
            await MordekaiserCardUtils.ExhaustMordekaiserCard(
                choiceContext,
                Owner.Player,
                Amount,
                PileType.Discard.GetPile(Owner.Player),
                cards => cards,
                "MORDEKAISER_NO_EXHAUST_DISCARD"
            );
        }
    }
}