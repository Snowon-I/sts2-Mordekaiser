using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.power;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_ability_obliterate() : CardModel(0, CardType.Attack, CardRarity.Ancient, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(10m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(MordekaiserObliterateAttack)
    ];
    
    private static decimal MordekaiserObliterateAttack(CardModel card,Creature? creature)
    {
        if (!card.IsInCombat || card.Owner.Creature.CombatState == null )
            return 0m;
        var MordekaiserObliterateAttackNum = card.IsUpgraded? 6m : 4m;
        var enemies = card.Owner.Creature.CombatState!.HittableEnemies;
        if (!enemies.Any(c => c.HasPower<Mordekaiser_deceasedsdomainpower>()))
            return enemies.Count(c => c.IsAlive) == 1 ? MordekaiserObliterateAttackNum : 0m;
        return enemies.Any(c => c.GetPower<Mordekaiser_deceasedsdomainpower>()!.Applier == card.Owner.Creature) ? MordekaiserObliterateAttackNum : 0m;
    }
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .WithAttackerAnim("Obliterate",0.1f,Owner.Creature)
            .TargetingAllOpponents(CombatState!)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(5m);
    }
    
}