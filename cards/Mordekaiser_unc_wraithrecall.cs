using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_unc_wraithrecall() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt,0,DynamicVars.Cards.IntValue);
        var cards = PileType.Exhaust.GetPile(Owner).Cards.Where(c => c.Type == CardType.Skill).OrderBy(c => c.Rarity).ThenBy(c => c.Id).ToList();
        var cardModel = await CardSelectCmd.FromSimpleGrid(choiceContext, cards, Owner, prefs);
        await CardPileCmd.Add(cardModel, PileType.Hand);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
    
}