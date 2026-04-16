using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_unc_wraithcycle() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var skillCards = PileType.Discard.GetPile(Owner).Cards.Where(c => c.Type == CardType.Skill);
        await CardPileCmd.Add(skillCards, PileType.Draw);
        await CardPileCmd.Shuffle(choiceContext, Owner);
        var topCards  = PileType.Draw.GetPile(Owner).Cards.Where(_ => true).Take(DynamicVars.Cards.IntValue).ToList();
        foreach (var card in topCards)
            await CardCmd.Exhaust(choiceContext,card);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
    
}