using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.power;

public class Mordekaiser_unyieldingpresencepower : PowerModel
{
    
    private bool _cantDied = true;
    
    private bool _lockLife = false;

    private decimal _hp;

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress)
        {
            return amount;
        }
        if (target != Owner)
        {
            return amount;
        }
        if (_lockLife)
        {
            return 0;
        }
        return amount;
    }

    public override bool ShouldDieLate(Creature creature)
    {
        if (creature != Owner)
        {
            return true;
        }
        if (!_cantDied)
        {
            return true;
        }
        return false;
    }
    
    public override decimal ModifyDamageCap(Creature? target, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner && !_lockLife )
        {
            return decimal.MaxValue;
        }
        return 0;
    }
    
    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner) return Task.CompletedTask;
        if (result.UnblockedDamage >= Owner.CurrentHp)
        {
            _hp = Owner.CurrentHp;
        }
        return Task.CompletedTask;
    }
    
    public override async Task AfterPreventingDeath(Creature creature)
    {
        if (creature != Owner) return;
        _lockLife = true;
        await CreatureCmd.SetCurrentHp(Owner,_hp);
        Owner.ShowsInfiniteHp = true;
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side) return;
        if (_lockLife)
        {
            if (Amount > 1)
               await PowerCmd.ModifyAmount(this,-1m,Owner,null);
            else
            {
                _cantDied = false;
                await CreatureCmd.Kill(Owner);
            }
        }
    }
    
}