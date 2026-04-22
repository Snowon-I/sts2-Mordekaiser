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

public class Mordekaiser_potentialblockpower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props,
        Creature target, CardModel? cardSource)
    {
        if (dealer != Owner || target.Side == Owner.Side ) return Task.CompletedTask;
        var _getnum = result.UnblockedDamage * 0.5m;
        SetAmount(Amount + (int)_getnum,true);
        //await PowerCmd.ModifyAmount(this, _getnum, Owner, null,true);
        Flash();
        return Task.CompletedTask;
    }

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result,
        ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (result.WasFullyBlocked ) return Task.CompletedTask;
        var MordekaiserReceivedGainPb = result.UnblockedDamage;
        SetAmount(Amount + MordekaiserReceivedGainPb,true);
        //await PowerCmd.ModifyAmount(this, MordekaiserReceivedGainPb, Owner, null,true);
        Flash();
        return Task.CompletedTask;
    }
    
    public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player || Amount <= 10) return Task.CompletedTask;
        SetAmount(Amount - 10,true);
        Flash();
        return Task.CompletedTask;
        //await PowerCmd.ModifyAmount(this, -10, Owner, null,true);
    }
    
    public override Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side || Amount <= 10) return Task.CompletedTask;
        SetAmount(Amount - 10,true);
        Flash();
        return Task.CompletedTask;
        //await PowerCmd.ModifyAmount(this, -10, Owner, null,true);
    }
    
}