using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.cards;

public class Mordekaiser_rare_hellecho() : CardModel(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0m),
        new ExtraDamageVar(8m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((c, _) => PileType.Exhaust.GetPile(c.Owner).Cards.Count)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        await MordekaiserCardUtils.DrawMordekaiserTypeCard(
            choiceContext,
            Owner,
            PileType.Exhaust.GetPile(Owner).Cards.Count,
            PileType.Exhaust.GetPile(Owner), PileType.Discard.GetPile(Owner),
            cards => cards.Where(_ => true),
            false
        );
    }

    public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(4m);
    }
    
}