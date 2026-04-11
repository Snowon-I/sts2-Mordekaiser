using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Mordekaiser.scripts;

namespace Mordekaiser.cards;

public class Mordekaiser_com_wraithfear() : CardModel(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
{
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [MordekaiserKeyWord.MordekaiserQuiesce];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new ("Power",1m),
        new ("quiescePower",2m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<VulnerablePower>(cardPlay.Target, DynamicVars["Power"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(cardPlay.Target, DynamicVars["Power"].BaseValue, Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars["Power"].UpgradeValueBy(1m);
        DynamicVars["quiescePower"].UpgradeValueBy(1m);
    }
    
    public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            await PowerCmd.Apply<VulnerablePower>(CombatState.HittableEnemies, DynamicVars["quiescePower"].BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<WeakPower>(CombatState.HittableEnemies, DynamicVars["quiescePower"].BaseValue, Owner.Creature, this);
        }
            
    }
    
}