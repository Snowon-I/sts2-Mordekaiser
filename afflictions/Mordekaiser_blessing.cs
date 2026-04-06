using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.scripts;

namespace Mordekaiser.afflictions;

public class Mordekaiser_blessing_com : AfflictionModel
{
    public override bool IsStackable => true;

    public override bool HasExtraCardText => true;
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == Card)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            var addCard = Card.Owner.Character.CardPool.GetUnlockedCards(Card.Owner.UnlockState, Card.Owner.RunState.CardMultiplayerConstraint).Where(c => c.Type is CardType.Attack or CardType.Skill && c.Rarity is CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare).TakeRandom(5,Card.Owner.RunState.Rng.CombatCardSelection).First();
            await CardCmd.Afflict<Mordekaiser_blessing_com>(addCard,1);
        }
    }
    
}

public class Mordekaiser_blessing_upgrade : AfflictionModel
{
    public override bool IsStackable => true;

    public override bool HasExtraCardText => true;
    
    public override void AfterApplied()
    {
        Card.EnergyCost.SetThisCombat(0);
    }
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == Card)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            var addCard = Card.Owner.Character.CardPool.GetUnlockedCards(Card.Owner.UnlockState, Card.Owner.RunState.CardMultiplayerConstraint).Where(c => c.Type is CardType.Attack or CardType.Skill && c.Rarity is CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare).TakeRandom(5,Card.Owner.RunState.Rng.CombatCardSelection).First();
            await CardCmd.Afflict<Mordekaiser_blessing_upgrade>(addCard,1);
        }
    }
    
}
