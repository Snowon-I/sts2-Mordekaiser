using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.power;

public class Mordekaiser_potentialblock : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task AfterAttack(AttackCommand command)
    {
        if (command.Attacker != Owner || command.TargetSide == Owner.Side ) return;
        var MordekaiserAttackGainPB = new Dictionary<Creature, List<DamageResult>>();
        foreach (var result in command.Results)
        {
            if (result.Receiver.IsEnemy)
            {
                if (!MordekaiserAttackGainPB.TryGetValue(result.Receiver, out var value))
                {
                    value = [];
                    MordekaiserAttackGainPB.Add(result.Receiver, value);
                }

                value.Add(result);
            }
        }
        foreach (var Creature in MordekaiserAttackGainPB.Keys)
        {
            var num = (int)Math.Floor(MordekaiserAttackGainPB[Creature].Sum(r => r.TotalDamage * 0.1));
            await PowerCmd.ModifyAmount(this, num, Owner, null);
        }
        Flash();
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result,
        ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (result.WasFullyBlocked ) return;
        var MordekaiserReceivedGainPb = result.UnblockedDamage;
        await PowerCmd.ModifyAmount(this, MordekaiserReceivedGainPb, Owner, null);
        Flash();
    }
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        if (Amount <= 10) return;
        Flash();
        await PowerCmd.ModifyAmount(this, -10, Owner, null);
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side) return;
        if (Amount <= 10) return;
        Flash();
        await PowerCmd.ModifyAmount(this, -10, Owner, null);
    }
    
}