using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.power;

public class Mordekaiser_darknessrisepower : PowerModel
{
    
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(0m, ValueProp.Unpowered)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Mordekaiser_darkenergypower>()];

    public override bool IsInstanced => true;

    public override int DisplayAmount => (int)Math.Max(0m, _num);
    
    private bool Mordekaiser_playAttack;
    
    private bool Mordekaiser_DEGetAll;
    
    private decimal _num;
    
    private CardModel? _currentPlayingCard;
    
    private static readonly Dictionary<(Creature creature, string sfxPath), DateTime> _sfxCooldown = new();
    
    private const int SfxCooldownMs = 100;
    
    private void PlaySfxSingle(string sfxPath)
    {
        var now = DateTime.Now;
        var key = (Owner, sfxPath);
        if (_sfxCooldown.TryGetValue(key, out var endTime) && now <= endTime) return;
        SfxCmd.Play(sfxPath);
        _sfxCooldown[key] = now.AddMilliseconds(SfxCooldownMs);
    }
    
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
            await CreatureCmd.Damage(context,enemy,DynamicVars.Damage, Owner);
        }
    }
    
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return Task.CompletedTask;
        if (cardPlay.Card.Type == CardType.Attack)
            _currentPlayingCard = cardPlay.Card;
        return Task.CompletedTask;
    }
    
    public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result,
        ValueProp props, Creature? target, CardModel? cardSource)
    {
        if (dealer != Owner || cardSource == null || _currentPlayingCard != cardSource) return Task.CompletedTask;
        if (result.TotalDamage >= 0m)
            Mordekaiser_playAttack = true;
        if (_num < 3m )
        {
            if (_num == 1m && !Mordekaiser_DEGetAll)
            {
                PlaySfxSingle("event:/Mordekaiser/Mordekaiser_darkness_second");
            }
            if (!Mordekaiser_DEGetAll)
                _num += 1m;
            else
                _num = 3m;
            InvokeDisplayAmountChanged();
        }
        if (_num != 3m || Mordekaiser_DEGetAll) return Task.CompletedTask;
        PlaySfxSingle("event:/Mordekaiser/Mordekaiser_darkness_trigger");
        Mordekaiser_DEGetAll = true;
        return Task.CompletedTask;
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return;
        if (cardPlay.Card.Type == CardType.Attack && cardPlay.Card == _currentPlayingCard && Mordekaiser_playAttack )
        { }
        else
        {
            if (Mordekaiser_DEGetAll && _num == 1m )
            {
                await Mordekaiser_EnemyHpCount(context);
                Mordekaiser_DEGetAll = false;
            }
            if (_num > 0m)
            {
                _num -= 1m;
                if (_num == 0m)
                    Mordekaiser_DEGetAll = false;
            }
            InvokeDisplayAmountChanged();
        }
        _currentPlayingCard = null;
        Mordekaiser_playAttack = false;
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
            await Mordekaiser_EnemyHpCount(choiceContext);
        if (_num > 0m)
        {
            _num -= 1m;
            if (_num == 0m)
                Mordekaiser_DEGetAll = false;
        }
        InvokeDisplayAmountChanged();
    }
    
    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side) return;
        if (Mordekaiser_DEGetAll)
            await Mordekaiser_EnemyHpCount(choiceContext);
        if (_num > 0m)
        {
            _num -= 1m;
            if (_num == 0m)
                Mordekaiser_DEGetAll = false;
        }
        InvokeDisplayAmountChanged();
    }

}

public class Mordekaiser_darkenergypower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool IsInstanced => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(0m, ValueProp.Unpowered)
    ];

    private bool Mordekaiser_DEGetAll;
    
    private bool Mordekaiser_playAttack;
    
    private CardModel? _currentPlayingCard;
    
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
            await CreatureCmd.Damage(context,enemy,DynamicVars.Damage, Owner);
        }
    }
    
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return Task.CompletedTask;
        if (cardPlay.Card.Type == CardType.Attack)
            _currentPlayingCard = cardPlay.Card;
        return Task.CompletedTask;
    }
    
    public override Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result,
        ValueProp props, Creature? target, CardModel? cardSource)
    {
        if (dealer != Owner || _currentPlayingCard != cardSource) return Task.CompletedTask;
        if (result.TotalDamage >= 0m)
        {
            Mordekaiser_playAttack = true;
        }
        if (Amount < 3)
        {
            if (Amount == 1 && !Mordekaiser_DEGetAll)
                SfxCmd.Play("event:/Mordekaiser/Mordekaiser_darkness_second");
            if (!Mordekaiser_DEGetAll)
                SetAmount(Amount + 1);
            else
                SetAmount(3);
        }
        if (Amount != 3 || Mordekaiser_DEGetAll) return Task.CompletedTask;
        SfxCmd.Play("event:/Mordekaiser/Mordekaiser_darkness_trigger");
        Mordekaiser_DEGetAll = true;
        return Task.CompletedTask;
    }
    
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner.Creature != Owner) return;
        if (cardPlay.Card.Type == CardType.Attack && cardPlay.Card == _currentPlayingCard && Mordekaiser_playAttack )
        { }
        else
        {
            if (Mordekaiser_DEGetAll && Amount == 1 )
            {
                await Mordekaiser_EnemyHpCount(context);
                await PowerCmd.ModifyAmount(this, -1, Owner, null,true);
                return;
            }
            await PowerCmd.ModifyAmount(this, -1, Owner, null,true);
        }
        _currentPlayingCard = null;
        Mordekaiser_playAttack = false;
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
