using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Mordekaiser.relics;

namespace Mordekaiser.cards;

public class Mordekaiser_rare_tearsoul() : CardModel(3, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,CardKeyword.Ethereal];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Power",3m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var mordekaiserrelic = Owner.GetRelic<Mordekaiser_relic>();
        if (mordekaiserrelic != null)
            mordekaiserrelic.MonsterSouls++;
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await PowerCmd.Apply<VulnerablePower>(cardPlay.Target, DynamicVars["Power"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<WeakPower>(cardPlay.Target, DynamicVars["Power"].BaseValue, Owner.Creature, this);
    }

    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        CardCmd.RemoveKeyword(this,CardKeyword.Ethereal);
    }
}