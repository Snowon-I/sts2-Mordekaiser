using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.cards;

namespace Mordekaiser.power;

public class Mordekaiser_doomsoulreleasepower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side) return;
        if (Owner.Player == null) return;
        var _cards =
            Owner.Player.Piles.FirstOrDefault(p => p.Type == PileType.Exhaust)!.Cards.Where(c =>
                c.Type is CardType.Attack or CardType.Skill or CardType.Power && c.Id != ModelDb.Card<Mordekaiser_ability_indestructible_live>().Id).TakeRandom(Amount, Owner.Player.RunState.Rng.CombatCardSelection).ToList();
        
        foreach (var _card in _cards)
        {
            bool exist_target;
            Creature? target;
            if (Owner.CombatState == null) return;
            switch (_card.TargetType)
            {
                case TargetType.RandomEnemy:
                case TargetType.AnyEnemy:
                    exist_target = true;
                    target = Owner.CombatState.Enemies.FirstOrDefault();
                    break;
                case TargetType.AnyPlayer:
                case TargetType.AnyAlly:
                    exist_target = true;
                    target = Owner.CombatState.PlayerCreatures.FirstOrDefault();
                    break;
                case TargetType.TargetedNoCreature:
                    exist_target = true;
                    target = Owner.CombatState.Creatures.FirstOrDefault();
                    break;
                case TargetType.AllAllies:
                case TargetType.None:
                case TargetType.Self:
                case TargetType.AllEnemies:
                case TargetType.Osty:
                default:
                    exist_target = false;
                    target = null;
                    break;
            }
            if (exist_target && target == null) return;
            await CardCmd.AutoPlay(
                choiceContext,
                _card,
                target
            );
        }
    }
    
}