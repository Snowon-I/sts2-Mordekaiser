using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_com_darkdrain() : CardModel(1, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var drawnCards = await MordekaiserCardUtils.DrawMordekaiserTypeCard(
            choiceContext,
            Owner,
            DynamicVars.Cards.IntValue,
            PileType.Exhaust.GetPile(Owner),PileType.Hand.GetPile(Owner),
            card => card.Where(c => c.Type == CardType.Attack),
            false
            );
        foreach (var card in drawnCards.Where(c => c.Keywords.Contains(CardKeyword.Exhaust)))
        {
            card.RemoveKeyword(CardKeyword.Exhaust); 
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    
}