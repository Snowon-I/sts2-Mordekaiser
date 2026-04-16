using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_unc_shadowconvert() : CardModel(1, CardType.Skill, CardRarity.Uncommon,TargetType.AnyEnemy)
{
    private TargetType _cardTargetType = TargetType.AnyEnemy;
            
    public override TargetType TargetType => _cardTargetType;
        
    protected override IEnumerable<DynamicVar> CanonicalVars => [];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<WeakPower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (!IsUpgraded)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            var _vulnerablePowerNum = cardPlay.Target.GetPowerAmount<VulnerablePower>();
            var _weakPowerNum = cardPlay.Target.GetPowerAmount<WeakPower>();
            await PowerCmd.Apply<StrengthPower>(cardPlay.Target, -_vulnerablePowerNum,Owner.Creature,this);
            await PowerCmd.Apply<DexterityPower>(cardPlay.Target, -_weakPowerNum,Owner.Creature,this);
            await PowerCmd.Remove<VulnerablePower>(cardPlay.Target);
            await PowerCmd.Remove<WeakPower>(cardPlay.Target);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(CombatState);
            foreach (var target in CombatState.HittableEnemies)
            {
                var _vulnerablePowerNum = target.GetPowerAmount<VulnerablePower>();
                var _weakPowerNum = target.GetPowerAmount<WeakPower>();
                await PowerCmd.Apply<StrengthPower>(target, -_vulnerablePowerNum,Owner.Creature,this);
                await PowerCmd.Apply<DexterityPower>(target, -_weakPowerNum,Owner.Creature,this);
                await PowerCmd.Remove<StrengthPower>(target);
                await PowerCmd.Remove<WeakPower>(target);
            }
        }
    }

    protected override void OnUpgrade()
    {
        _cardTargetType = TargetType.AllEnemies;
    }
    
}
