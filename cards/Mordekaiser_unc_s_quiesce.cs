using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.power;
using Mordekaiser.scripts;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_unc_soulfollow() : CardModel(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MordekaiserKeyWord.MordekaiserQuiesce];
    
    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(7m, ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            await CardPileCmd.Add(this, PileType.Hand);
        }
    }
    
}

public sealed class Mordekaiser_unc_wraithwarcry() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MordekaiserKeyWord.MordekaiserQuiesce];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await MordekaiserCardUtils.DrawMordekaiserTypeCard(
            choiceContext,
            Owner,
            DynamicVars.Cards.IntValue,
            PileType.Draw.GetPile(Owner),PileType.Hand.GetPile(Owner),
            card => card.Where(c => c.Type == CardType.Attack),
            false
        );
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            var targetcard = await MordekaiserCardUtils.DrawMordekaiserTypeCard(
                choiceContext,
                Owner,
                DynamicVars.Cards.IntValue,
                PileType.Discard.GetPile(Owner),PileType.Hand.GetPile(Owner),
                cards => cards.Where(c => c.Type == CardType.Attack),
                false,
                noCost:true
            );
            if (targetcard.Count > 0)
                foreach (var c in targetcard)
                    CardCmd.ApplyKeyword(c, CardKeyword.Exhaust);
        }
    }
    
}

public sealed class Mordekaiser_unc_dashstance() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar(10m,ValueProp.Move),
        new CardsVar(1),
        new ("quiesceVar", 2)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Mordekaiser_dashstancepower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<Mordekaiser_dashstancepower>(Owner.Creature, DynamicVars.Cards.IntValue,Owner.Creature,this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(2m);
        DynamicVars.Cards.UpgradeValueBy(1);
        DynamicVars["quiesceVar"].UpgradeValueBy(1);
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            await PowerCmd.Apply<Mordekaiser_dashstancepower>(Owner.Creature, DynamicVars["quiesceVar"].IntValue,Owner.Creature,this);
        }
    }
    
}

public sealed class Mordekaiser_unc_soulbarrier() : CardModel(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,MordekaiserKeyWord.MordekaiserQuiesce];
    
    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(18m, ValueProp.Move)];
    
    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
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
