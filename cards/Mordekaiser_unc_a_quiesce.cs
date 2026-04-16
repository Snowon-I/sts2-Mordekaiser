using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.scripts;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_unc_wraithsummonstrike() : CardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [MordekaiserKeyWord.MordekaiserQuiesce];
    
    public override IEnumerable<CardTag> Tags => [CardTag.Strike];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        
        await MordekaiserCardUtils.ExhaustMordekaiserCard(choiceContext, Owner, 1, PileType.Draw.GetPile(Owner),
            cards => cards.Where(c => c.Type == CardType.Attack));
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            await MordekaiserCardUtils.DrawMordekaiserTypeCard(
                choiceContext, Owner, 
                1, 
                PileType.Exhaust.GetPile(Owner), PileType.Hand.GetPile(Owner),
                cards => cards.Where(c => c.Type == CardType.Attack),
                false);
        }
    }
    
}

public sealed class Mordekaiser_unc_sweepbreak() : CardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(9m, ValueProp.Move)];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MordekaiserKeyWord.MordekaiserQuiesce];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(CombatState)
                .Execute(choiceContext);
        }
    }
    
}

public sealed class Mordekaiser_unc_defendwraith() : CardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    
    protected override bool ShouldGlowGoldInternal => WasCardQuiesceLastCard;

    private bool WasCardQuiesceLastCard {
        get
        {
            var _lastcard = CombatManager.Instance.History.CardPlaysStarted.LastOrDefault(c => c.CardPlay.Card.Owner == Owner && c.CardPlay.Card != this);
            return _lastcard != null && _lastcard.CardPlay.Card.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce);
        }
        
    }    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(13m, ValueProp.Move),
        new BlockVar(13m,ValueProp.Move)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MordekaiserKeyWord.MordekaiserQuiesce];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        if (WasCardQuiesceLastCard)
        {
            await CardCmd.Exhaust(choiceContext,this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
        DynamicVars.Block.UpgradeValueBy(5m);
    }
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState, card);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block,null);
        }
    }
    
}







