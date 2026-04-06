using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.cards;

public class Mordekaiser_com_fleshreap() : CardModel(2, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => 
    [
        new DamageVar(9m, ValueProp.Move), 
        new PowerVar<VulnerablePower>(2m)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VulnerablePower>()];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(CombatState!)
            .Execute(choiceContext);
        await PowerCmd.Apply<VulnerablePower>(CombatState!.HittableEnemies, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);

    }
    
    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m);
        DynamicVars.Vulnerable.UpgradeValueBy(1m);
    }
}