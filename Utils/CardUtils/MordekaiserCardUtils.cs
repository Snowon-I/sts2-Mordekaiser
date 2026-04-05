using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.scripts;

namespace Mordekaiser.Utils.CardUtils;

public class MordekaiserCardUtils
{
    
    //封装 将某牌库某类型几张牌放入哪（drawCard判定 triggerMordekaiserQuiesce判定）
    public static async Task<List<CardModel>> DrawMordekaiserTypeCard(
        PlayerChoiceContext choiceContext,
        Player Owner,int cardnum,
        CardPile cardPile,
        CardPile targetPile,
        Func<IEnumerable<CardModel>, IEnumerable<CardModel>> howGetCard, 
        bool drawCard,
        string noDrawMessage = "MORDEKAISER_NO_DRAW",
        bool triggerMordekaiserQuiesce = false,
        bool noCost = false
        )
    {
        var targetCard = howGetCard(cardPile.Cards).TakeRandom(cardnum,Owner.RunState.Rng.CombatCardSelection).ToList();
        if (targetCard.Count == 0)
            ThinkCmd.Play(new LocString("combat_messages", noDrawMessage), Owner.Creature, 2.0);
        if (targetCard.Count != 0)
        {
            await CardPileCmd.Add(targetCard, targetPile);
            if (drawCard)
                foreach (var card in targetCard)
                    await Hook.AfterCardDrawn(Owner.Creature.CombatState!, choiceContext, card, false);
            if (noCost)
                foreach (var card in targetCard)
                    card.EnergyCost.SetThisTurnOrUntilPlayed(0);
            if (triggerMordekaiserQuiesce)
            {
                foreach (var card in targetCard.Where(cards => cards.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce)))
                    await card.AfterCardExhausted(choiceContext, card, false);
            }
        }
        return targetCard;
    }

    //封装 将某牌库某类型几张牌消耗
    public static async Task ExhaustMordekaiserCard(
        PlayerChoiceContext choiceContext,
        Player Owner,int cardnum,
        CardPile cardPile,
        Func<IEnumerable<CardModel>, IEnumerable<CardModel>> howGetCard,
        string noDrawMessage = "MORDEKAISER_NO_EXHAUST"
    )
    {
        var targetCard = howGetCard(cardPile.Cards).TakeRandom(cardnum,Owner.RunState.Rng.CombatCardSelection).ToList();
        if (targetCard.Count == 0)
            ThinkCmd.Play(new LocString("combat_messages", noDrawMessage), Owner.Creature, 2.0);
        if (targetCard.Count != 0)
        {
            foreach (CardModel card in targetCard)
                await CardCmd.Exhaust(choiceContext, card);
        }
    }
    
    //封装 触发某个牌区某几张牌的寂灭
    public static async Task TriggerMordekaiserCardQuiesce(
        PlayerChoiceContext choiceContext,
        Player Owner,int cardnum,
        CardPile cardPile,
        Func<IEnumerable<CardModel>, IEnumerable<CardModel>> howGetCard,
        string noDrawMessage = "MORDEKAISER_NO_Quiesce"
    )
    {
        var targetCard = howGetCard(cardPile.Cards).TakeRandom(cardnum,Owner.RunState.Rng.CombatCardSelection).ToList();
        if (targetCard.Count == 0)
            ThinkCmd.Play(new LocString("combat_messages", noDrawMessage), Owner.Creature, 2.0);
        if (targetCard.Count != 0)
        {
            foreach (var card in targetCard.Where(cards => cards.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce)))
                await card.AfterCardExhausted(choiceContext, card, false);
        }
    }
    
}