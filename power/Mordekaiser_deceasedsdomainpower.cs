using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;


namespace Mordekaiser.power;

public class Mordekaiser_deceasedsdomainpower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new StringVar("Applier"),
    ];
    
    public override Task AfterApplied(Creature? applier, CardModel? cardSource)
    {
        if (applier != null)
            ((StringVar)DynamicVars["Applier"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, applier.Player!.NetId);
        return Task.CompletedTask;
    }
    
    public override decimal ModifyPowerAmountGiven(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress || cardSource != null && target == null) return amount;
        if (power.Id == ModelDb.Power<MinionPower>().Id) return amount ;
        var MordekaiserInSolo = giver.HasPower<Mordekaiser_deceasedsdomainpower>();
        if (target == null) return MordekaiserInSolo ? 0 : amount;
        
        var targetInMordekaiserISolo = target.HasPower<Mordekaiser_deceasedsdomainpower>();
        return !targetInMordekaiserISolo && !MordekaiserInSolo ? amount : targetInMordekaiserISolo && MordekaiserInSolo ? amount : 0;
    }
    
    public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier, out decimal modifiedAmount)
    {
        if (!CombatManager.Instance.IsInProgress)
        {
            modifiedAmount = amount;
            return true;
        }
        if (canonicalPower.Id == ModelDb.Power<MinionPower>().Id)
        {
            modifiedAmount = amount;
            return true;
        } 
        var targetInMordekaiserISolo = target.HasPower<Mordekaiser_deceasedsdomainpower>();
        if (applier == null)
        {
            if (targetInMordekaiserISolo)
            {
                modifiedAmount = 0;
                return false;
            }
            modifiedAmount = amount;
            return true;
        }
        var MordekaiserInSolo = applier.HasPower<Mordekaiser_deceasedsdomainpower>();
        if ((!targetInMordekaiserISolo && !MordekaiserInSolo) || (targetInMordekaiserISolo && MordekaiserInSolo))
        {
            modifiedAmount = amount;
            return true;
        }
        modifiedAmount = 0;
        return false;
    }

    public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power.Applier == Owner && power.Id == ModelDb.Power<MinionPower>().Id)
        {
            var powerd = ModelDb.Power<Mordekaiser_deceasedsdomainpower>().ToMutable();
            await PowerCmd.Apply(powerd,power.Owner, Amount, null, null);
            powerd.Applier = Applier; 
            if (powerd.Applier != null)
                ((StringVar)powerd.DynamicVars["Applier"]).StringValue = PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, powerd.Applier.Player!.NetId);
        }
    }

    public override decimal ModifyHpLostAfterOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress)
            return amount;
        var targetInMordekaiserISolo = target.HasPower<Mordekaiser_deceasedsdomainpower>();
        if (dealer == null)
            return targetInMordekaiserISolo ? 0 : amount;
        var MordekaiserInSolo = dealer.HasPower<Mordekaiser_deceasedsdomainpower>();
        
        return !MordekaiserInSolo && !targetInMordekaiserISolo ? amount : MordekaiserInSolo && targetInMordekaiserISolo ? amount : 0;
    }
    
    public override decimal ModifyDamageCap(Creature? target, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress || cardSource != null && target == null)
            return decimal.MaxValue;

        if (target == null)
        {
            if (dealer == null) return decimal.MaxValue;
            var MordekaiserInSolo = dealer.HasPower<Mordekaiser_deceasedsdomainpower>();
            return !MordekaiserInSolo ? decimal.MaxValue : 0;
        }
        
        var targetInMordekaiserISolo = target.HasPower<Mordekaiser_deceasedsdomainpower>();
        if (dealer != null)
        {
            var MordekaiserInSolo = dealer.HasPower<Mordekaiser_deceasedsdomainpower>();
            return !MordekaiserInSolo && !targetInMordekaiserISolo ? decimal.MaxValue : MordekaiserInSolo && targetInMordekaiserISolo ? decimal.MaxValue : 0;
        }
        return decimal.MaxValue;
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == CombatSide.Enemy)
        {
            await PowerCmd.ModifyAmount(this, -1, Owner, null);
            if (Owner.Side == CombatSide.Player)
            {
                var enemie = Owner.CombatState?.Enemies.Where(e => e.GetPower<Mordekaiser_deceasedsdomainpower>()?.Applier == Owner ).ToList();
                if (enemie == null || enemie.Count == 0)
                    await PowerCmd.Remove<Mordekaiser_deceasedsdomainpower>(Owner);
            }
        }
    }

    public override async Task AfterRemoved(Creature oldOwner)
    {
        if (oldOwner.IsEnemy)
        {
            var player = Applier;
            if (player != null)
            {
                var enemie = Owner.CombatState?.Enemies.Where(e => e.GetPower<Mordekaiser_deceasedsdomainpower>()?.Applier == player ).ToList();
                if (enemie == null || enemie.Count == 0)
                    await PowerCmd.Remove<Mordekaiser_deceasedsdomainpower>(player);
            }
        }
        else
        {
            var enemie = Owner.CombatState?.Enemies.Where(e => e.GetPower<Mordekaiser_deceasedsdomainpower>()?.Applier == oldOwner ).ToList();
            if (enemie is { Count: > 0 }) 
                foreach (var e in enemie)
                    await PowerCmd.Remove<Mordekaiser_deceasedsdomainpower>(e); 
        }
    }
}