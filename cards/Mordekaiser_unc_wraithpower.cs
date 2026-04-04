using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.cards;

public class Mordekaiser_unc_wraithpower() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await MordekaiserCardUtils.DrawMordekaiserTypeCard(
            choiceContext,
            Owner,
            (int)DynamicVars.Cards.BaseValue,
            PileType.Draw.GetPile(Owner), PileType.Hand.GetPile(Owner),
            cards => cards.Where(c => c.Keywords.Contains(CardKeyword.Exhaust)), 
            true, 
            "MORDEKAISER_NO_DRAW_EXHAUST",
            true);
    }

    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
    
}