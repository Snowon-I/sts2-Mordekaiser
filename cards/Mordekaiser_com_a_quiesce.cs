using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.scripts;

namespace Mordekaiser.cards;

public class Mordekaiser_com_quiesceattack() : CardModel(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(15m, ValueProp.Move)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust, MordekaiserKeyWord.MordekaiserQuiesce];
    
    public override IEnumerable<CardTag> Tags => [CardTag.Strike];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
    
    public override Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
        return Task.CompletedTask;
    }
    
}

public class Mordekaiser_com_unleashshadow() : CardModel(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(16m, ValueProp.Move),
        new BlockVar(8m,ValueProp.Move)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MordekaiserKeyWord.MordekaiserQuiesce];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
        DynamicVars.Block.UpgradeValueBy(4m);
    }
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block,null);
        }
    }
    
}

public class Mordekaiser_com_steelcharge() : CardModel(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(12m, ValueProp.Move),
        new PowerVar<StrengthPower>(1m)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MordekaiserKeyWord.MordekaiserQuiesce];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            await PowerCmd.Apply<StrengthPower>(card.Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
        }
    }
    
}

public class Mordekaiser_com_chargedhammerswing() : CardModel(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0m),
        new ExtraDamageVar(12m),
        new CalculationExtraVar(14m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((model, _) => model.EnergyCost.GetWithModifiers(CostModifiers.All)),
        new CalculatedVar("quiescedamagevar").WithMultiplier((model, _) => model.EnergyCost.GetWithModifiers(CostModifiers.All))
    ];
    // * (model.IsUpgraded? 14m/12m : 16m/14m)
    public override Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
    {
        if (card != this || Pile is not { Type: PileType.Hand } || NCard.FindOnTable(card) == null )
            return Task.CompletedTask;
        EnergyCost.SetThisCombat(Owner.RunState.Rng.CombatEnergyCosts.NextInt(4));
        NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
        return Task.CompletedTask;
    }
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MordekaiserKeyWord.MordekaiserQuiesce];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        DynamicVars.ExtraDamage.UpgradeValueBy(2m);
        DynamicVars.CalculationExtra.UpgradeValueBy(2m);
    }
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            var target = CombatState!.HittableEnemies[0];
            ArgumentNullException.ThrowIfNull(target);
            await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(this)
                .Targeting(target)
                .Execute(choiceContext);
        }
    }
    
}
