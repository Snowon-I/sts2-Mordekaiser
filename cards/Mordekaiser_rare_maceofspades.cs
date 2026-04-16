using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_rare_maceofspades() : CardModel(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override bool ShouldGlowGoldInternal => WasCardShouldExhaust;

    private bool WasCardShouldExhaust => DynamicVars["multtime"].BaseValue == 2m;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(6m, ValueProp.Move),
        new("multtime",0m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        if (cardPlay.Card.DynamicVars["multtime"].BaseValue >= 2m)
            await CardCmd.Exhaust(choiceContext,cardPlay.Card);
        else
            await CardPileCmd.Add(cardPlay.Card, PileType.Draw, CardPilePosition.Top);
        cardPlay.Card.DynamicVars.Damage.UpgradeValueBy(DynamicVars.Damage.BaseValue);
        cardPlay.Card.DynamicVars["multtime"].UpgradeValueBy(1m);
        
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
    
}