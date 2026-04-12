using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_unc_breakpowerhammer() : CardModel(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(25m, ValueProp.Move)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<WeakPower>()
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        var targethasVulnerable = cardPlay.Target.HasPower<VulnerablePower>();
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        if (targethasVulnerable)
        {
            await PowerCmd.Apply<StrengthPower>(Owner.Creature, 2m, Owner.Creature, this);
            if (cardPlay.Target != null)
                await PowerCmd.Apply<WeakPower>(cardPlay.Target, 2m, Owner.Creature, this);
        }
    }
    
    public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";
    
    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    
}