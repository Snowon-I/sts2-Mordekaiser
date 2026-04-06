using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.power;


namespace Mordekaiser.cards;

public class Mordekaiser_unc_shadowdrain() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        await PowerCmd.Apply<Mordekaiser_shadowdrainpower>(CombatState.HittableEnemies,
            ((CalculatedVar)DynamicVars["Power"]).Calculate(Owner.Creature), Owner.Creature, this);
    }
    
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(1);
    }
    
    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";
    
}