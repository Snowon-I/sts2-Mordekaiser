using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.cards;

public class Mordekaiser_ability_obliterate() : CardModel(0, CardType.Attack, CardRarity.Token, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(12m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) => MordekaiserObliterateAttack(card))
    ];
    
    private static decimal MordekaiserObliterateAttack(CardModel card)
    {
        decimal MordekaiserObliterateAttackNum = card.IsUpgraded? 6m : 4m;
        if (card.CombatState!.Enemies.Count(c => c.IsAlive) == 1)
        {
            return MordekaiserObliterateAttackNum;
        }
        return 0m;
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .WithAttackerAnim("Obliterate",0.1f,base.Owner.Creature)
            .TargetingAllOpponents(CombatState!)
            .Execute(choiceContext);
    }
    
    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(5m);
    }
    
}