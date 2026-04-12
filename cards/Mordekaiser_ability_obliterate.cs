using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_ability_obliterate() : CardModel(0, CardType.Attack, CardRarity.Ancient, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(12m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) => MordekaiserObliterateAttack(card))
    ];
    
    private static decimal MordekaiserObliterateAttack(CardModel card)
    {
        decimal MordekaiserObliterateAttackNum = card.IsUpgraded? 6m : 4m;
        return card.CombatState!.HittableEnemies.Count(c => c.IsAlive) == 1 ? MordekaiserObliterateAttackNum : 0m;
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .WithAttackerAnim("Obliterate",0.1f,Owner.Creature)
            .TargetingAllOpponents(CombatState!)
            .Execute(choiceContext);
    }
    
    public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(5m);
    }
    
}