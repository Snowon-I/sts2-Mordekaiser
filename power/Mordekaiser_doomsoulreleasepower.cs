using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mordekaiser.power;

public class Mordekaiser_doomsoulreleasepower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        var _cards =
            player.Piles.FirstOrDefault(p => p.Type == PileType.Exhaust)!.Cards.Where(c =>
                c.Type is CardType.Attack or CardType.Skill or CardType.Power).TakeRandom(Amount, player.RunState.Rng.CombatCardSelection).ToList();
        foreach (var _card in _cards)
        {
            Creature? target;
            switch (_card.TargetType)
            {
                case TargetType.AnyEnemy:
                    target = Owner.CombatState!.Enemies.FirstOrDefault();
                    break;
                case TargetType.AnyPlayer:
                case TargetType.AnyAlly:
                    target = Owner.CombatState!.PlayerCreatures.FirstOrDefault();
                    break;
                case TargetType.AllAllies:
                    target = null;
                    break;
                case TargetType.TargetedNoCreature:
                    target = Owner.CombatState!.Creatures.FirstOrDefault();
                    break;
                default:
                    target = null;
                    break;
            }
            await CardCmd.AutoPlay(
                choiceContext,
                _card,
                target
            );
        }
    }
    
}