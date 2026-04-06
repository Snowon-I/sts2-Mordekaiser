using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.power;
using Mordekaiser.scripts;

namespace Mordekaiser.cards;

public class Mordekaiser_ability_indestructible_block() : CardModel(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Mordekaiser_ability_indestructible_live>()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((card, _) => MordekaiserCardShowBlock(card))
    ];
    
    private static decimal MordekaiserCardShowBlock(CardModel card)
    {
        
        double MordekaiserMxHP = card.Owner.Creature.MaxHp;
        if (card.Owner.Creature.GetPower<Mordekaiser_potentialblock>() == null)
        {
            return Math.Floor((decimal)MordekaiserMxHP * 0.05m);
        }
        double MordekaiserDB = card.IsUpgraded? card.Owner.Creature.GetPowerAmount<Mordekaiser_potentialblock>() * 0.25 : card.Owner.Creature.GetPowerAmount<Mordekaiser_potentialblock>() * 0.2;
        if (MordekaiserDB / MordekaiserMxHP >= 0.05)
            return MordekaiserDB / MordekaiserMxHP >= 0.3 ? Math.Floor((decimal)MordekaiserMxHP * 0.3m) : Math.Floor((decimal)MordekaiserDB);
        return Math.Floor((decimal)MordekaiserMxHP * 0.05m);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature,"Cast",1f);
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(Owner.Creature),DynamicVars.CalculatedBlock.Props, cardPlay);
        CardModel Mordekaiser_indestructible = Owner.Creature.CombatState?.CreateCard<Mordekaiser_ability_indestructible_live>(Owner)!;
        await CardPileCmd.AddGeneratedCardToCombat(Mordekaiser_indestructible, PileType.Hand, addedByPlayer: true);
        await PowerCmd.Remove(Owner.Creature.GetPower<Mordekaiser_potentialblock>());
    }
    
    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";
}

public class Mordekaiser_ability_indestructible_live() : CardModel(0, CardType.Skill, CardRarity.Token, TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal, CardKeyword.Exhaust, MordekaiserKeyWord.MordekaiserQuiesce];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<Mordekaiser_ability_indestructible_block>()];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0m),
        new CalculationExtraVar(1m),
        new CalculatedVar("HealVar").WithMultiplier((card,_) => Math.Max(1m,MordekaiserCardHeal(card))),
    ];
    
    private static decimal MordekaiserCardHeal(CardModel card)
    {

        if (card.Owner.Creature.Block == 0)
            return 0m;
        var MordekaiserHeal = card.IsUpgraded? card.Owner.Creature.Block * 0.4 : card.Owner.Creature.Block * 0.3;
        return Math.Floor((decimal)MordekaiserHeal);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature,((CalculatedVar)DynamicVars["HealVar"]).Calculate(Owner.Creature));
        await CreatureCmd.LoseBlock(Owner.Creature, Owner.Creature.Block);
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this)
        { 
            await PowerCmd.Apply<Mordekaiser_potentialblock>(Owner.Creature,5m, Owner.Creature,null);
            CardModel Mordekaiser_indestructible = Owner.Creature.CombatState?.CreateCard<Mordekaiser_ability_indestructible_block>(Owner)!;
            await CardPileCmd.AddGeneratedCardToCombat(Mordekaiser_indestructible, PileType.Discard, addedByPlayer: true);
        }
    }
    
    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";
}
