using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.relics;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_rare_souldrain() : CardModel(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust,CardKeyword.Ethereal];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12m,ValueProp.Move)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var mordekaiserrelic = Owner.GetRelic<Mordekaiser_soulcrown>();
        var mordekaiserrelicorobas = Owner.GetRelic<Mordekaiser_soulcrown_orobas>();
        if (mordekaiserrelic != null)
            mordekaiserrelic.MonsterSouls++;
        if (mordekaiserrelicorobas != null)
            mordekaiserrelicorobas.MonsterSouls++;
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        CardCmd.RemoveKeyword(this,CardKeyword.Ethereal);
    }
    
}