using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_rare_wraithstorm() : CardModel(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new EnergyVar(2)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await MordekaiserCardUtils.DrawMordekaiserTypeCard(
            choiceContext,
            Owner,DynamicVars.Energy.IntValue,
            PileType.Exhaust.GetPile(Owner),PileType.Discard.GetPile(Owner),
            cards => cards.Where(_ => true), 
            false
            );
        await MordekaiserCardUtils.ExhaustMordekaiserCard(
            choiceContext,
            Owner,DynamicVars.Energy.IntValue,
            PileType.Discard.GetPile(Owner),
            cards => cards.Take(DynamicVars.Energy.IntValue)
            );
        await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Energy.UpgradeValueBy(1);
    }
    
}