using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

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
                c.Type is CardType.Attack or CardType.Skill or CardType.Power).TakeRandom(Amount, Owner.Player.RunState.Rng.CombatCardSelection).ToList();
        foreach (var _card in _cards)
        {
            Creature? target;
            switch (_card.TargetType)
            {
                case TargetType.AnyEnemy:
                    ArgumentNullException.ThrowIfNull(Owner.CombatState);
                    target = Owner.CombatState.Enemies.FirstOrDefault();
                    ArgumentNullException.ThrowIfNull(target);
                    break;
                case TargetType.AnyPlayer:
                case TargetType.AnyAlly:
                    ArgumentNullException.ThrowIfNull(Owner.CombatState);
                    target = Owner.CombatState.PlayerCreatures.FirstOrDefault();
                    ArgumentNullException.ThrowIfNull(target);
                    break;
                case TargetType.AllAllies:
                    target = null;
                    ArgumentNullException.ThrowIfNull(Owner.CombatState);
                    break;
                case TargetType.TargetedNoCreature:
                    ArgumentNullException.ThrowIfNull(Owner.CombatState);
                    target = Owner.CombatState.Creatures.FirstOrDefault();
                    ArgumentNullException.ThrowIfNull(target);
                    break;
                default:
                    target = null;
                    ArgumentNullException.ThrowIfNull(Owner.CombatState);
                    break;
            }
            ArgumentNullException.ThrowIfNull(Owner.CombatState);
            await CardCmd.AutoPlay(
                choiceContext,
                _card,
                target
            );
        }
    }
    
}