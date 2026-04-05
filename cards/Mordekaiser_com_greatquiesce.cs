using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.cards;

public class Mordekaiser_com_greatquiesce() : CardModel(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await MordekaiserCardUtils.ExhaustMordekaiserCard(
            choiceContext,
            Owner,
            DynamicVars.Cards.IntValue,
            PileType.Hand.GetPile(Owner),
            card => card
        );
        await MordekaiserCardUtils.ExhaustMordekaiserCard(
            choiceContext,
            Owner,
            DynamicVars.Cards.IntValue,
            PileType.Draw.GetPile(Owner),
            card => card
        );
        await MordekaiserCardUtils.ExhaustMordekaiserCard(
            choiceContext,
            Owner,
            DynamicVars.Cards.IntValue,
            PileType.Discard.GetPile(Owner),
            card => card
        );
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
    
    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";
    
}