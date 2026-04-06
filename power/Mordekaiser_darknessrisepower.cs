using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.power;

public class Mordekaiser_darknessrise : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return;
        if (Owner.CombatState == null) return;
        var darkenergy = Owner.GetPower<Mordekaiser_darkenergy>();
        if (cardPlay.Card.Type == CardType.Attack)
        {
            if (darkenergy == null)
            {
                darkenergy = await PowerCmd.Apply<Mordekaiser_darkenergy>(Owner, 1m, Owner, null);
            }
            else
            {
                await PowerCmd.ModifyAmount(darkenergy,1m, Owner,null);
            }
            if (darkenergy!.Amount == 3 && !darkenergy.Mordekaiser_DEGetAll )
            {
                darkenergy.Mordekaiser_DEGetAll = true;
            }
        }
        else
        {
            if (darkenergy == null) return;
            await PowerCmd.ModifyAmount(Owner.GetPower<Mordekaiser_darkenergy>()!, -1, Owner, null);
        }
    }

}

public class Mordekaiser_darkenergy : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(0m, ValueProp.Unpowered)
    ];

    public bool Mordekaiser_DEGetAll;
    
    private async Task Mordekaiser_EnemyHpCount(PlayerChoiceContext context)
    {
        if (Owner.CombatState == null) return;
        foreach (var enemy in Owner.CombatState.HittableEnemies)
        {
            DynamicVars.Damage.BaseValue = (decimal)Math.Floor(enemy.MaxHp * 0.01);
            if (DynamicVars.Damage.BaseValue < 1)
            {
                DynamicVars.Damage.BaseValue = 1;
            }
            await CreatureCmd.Damage(context,Owner.CombatState.HittableEnemies,DynamicVars.Damage, Owner);
        }
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return;
        if (!Mordekaiser_DEGetAll) return;
        await Mordekaiser_EnemyHpCount(context);
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result,
        ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner) return;
        if (!Mordekaiser_DEGetAll) return;
        await Mordekaiser_EnemyHpCount(choiceContext);
    }
    
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        if (Mordekaiser_DEGetAll)
        {
            await Mordekaiser_EnemyHpCount(choiceContext);
        }
        Flash();
        await PowerCmd.ModifyAmount(this, -1, Owner, null);
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side) return;
        if (Mordekaiser_DEGetAll)
        {
            await Mordekaiser_EnemyHpCount(choiceContext);
        }
        Flash();
        await PowerCmd.ModifyAmount(this, -1, Owner, null);
    }
}
