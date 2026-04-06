using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Mordekaiser.cards;

public class Mordekaiser_rare_doomimminent() : CardModel(1, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>(44m)
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        foreach (var enemy in CombatState.HittableEnemies)
        {
            await CreatureCmd.Stun(enemy);
            await PowerCmd.Apply<DexterityPower>(enemy, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        }
    }

    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(55m);
    }
    
}