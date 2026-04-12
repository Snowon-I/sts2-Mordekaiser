using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.afflictions;
using Mordekaiser.scripts;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_rare_underworldblessing() : CardModel(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,MordekaiserKeyWord.MordekaiserQuiesce];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override void OnUpgrade(){}
    
    public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";
    
    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null)
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            var addCard = Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint).Where(c => c.Type is CardType.Attack or CardType.Skill && c.Rarity is CardRarity.Common or CardRarity.Uncommon or CardRarity.Rare).TakeRandom(5,Owner.RunState.Rng.CombatCardSelection).First();
            var giveCard = Owner.Creature.CombatState?.CreateCard(addCard,Owner);
            if (giveCard == null) return;
            if (card.IsUpgraded)
                await CardCmd.Afflict<Mordekaiser_blessing_upgrade>(giveCard,1);
            else
                await CardCmd.Afflict<Mordekaiser_blessing_com>(giveCard,1);
            await CardPileCmd.AddGeneratedCardToCombat(giveCard, PileType.Hand, addedByPlayer: true);
        }
            
    }
    
}