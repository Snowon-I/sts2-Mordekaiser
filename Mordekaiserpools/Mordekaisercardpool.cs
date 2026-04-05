using Godot;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.cards;

namespace Mordekaiser.Mordekaiserpools;

public class Mordekaisercardpool : CardPoolModel
{
    public override string Title => "mordekaiser";

    public override string EnergyColorName => "mordekaiser";

    public override string CardFrameMaterialPath => "card_frame_red";

    public override Color DeckEntryCardColor => new Color("D62000");

    public override Color EnergyOutlineColor => new Color("802020");

    public override bool IsColorless => false;

    protected override CardModel[] GenerateAllCards() => [
        
        ModelDb.Card<Mordekaiser_ability_obliterate>(),
        ModelDb.Card<Mordekaiser_ability_deathsgrasp>(),
        ModelDb.Card<Mordekaiser_ability_indestructible_block>(),
        ModelDb.Card<Mordekaiser_ability_indestructible_live>(),
        
        ModelDb.Card<Mordekaiser_base_attack>(),
        ModelDb.Card<Mordekaiser_base_defend>(),
        ModelDb.Card<Mordekaiser_base_darknessrise>(),
        ModelDb.Card<Mordekaiser_base_criticalattack>(),
        
        //attack
        ModelDb.Card<Mordekaiser_com_attack>(),
        ModelDb.Card<Mordekaiser_com_crushslam>(),
        ModelDb.Card<Mordekaiser_com_agonytorment>(),
        ModelDb.Card<Mordekaiser_com_dualhammerslam>(),
        ModelDb.Card<Mordekaiser_com_fleshreap>(),
        ModelDb.Card<Mordekaiser_com_quiesceattack>(),
        ModelDb.Card<Mordekaiser_com_unleashshadow>(),
        ModelDb.Card<Mordekaiser_com_steelcharge>(),
        ModelDb.Card<Mordekaiser_com_chargedhammerswing>(),
        ModelDb.Card<Mordekaiser_com_steelstrike>(),
        ModelDb.Card<Mordekaiser_com_underworldstrike>(),
        
        //skill
        ModelDb.Card<Mordekaiser_com_dashforward>(),
        ModelDb.Card<Mordekaiser_com_darksoulbarrier>(),
        ModelDb.Card<Mordekaiser_com_wraithfear>(),
        ModelDb.Card<Mordekaiser_com_prepareblock>(),
        ModelDb.Card<Mordekaiser_com_sacrifice>(),
        ModelDb.Card<Mordekaiser_com_darkdrain>(),
        ModelDb.Card<Mordekaiser_com_unmovingironbody>(),
        ModelDb.Card<Mordekaiser_com_greatquiesce>(),

        
        //attack
        ModelDb.Card<Mordekaiser_unc_wraithpursuit>(),
        ModelDb.Card<Mordekaiser_unc_wraithsummonstrike>(),
        ModelDb.Card<Mordekaiser_unc_sweepbreak>(),
        ModelDb.Card<Mordekaiser_unc_soulthrust>(),
        ModelDb.Card<Mordekaiser_unc_wraithecho>(),
        ModelDb.Card<Mordekaiser_unc_wraithdrain>(),
        ModelDb.Card<Mordekaiser_unc_wraithseek>(),
        ModelDb.Card<Mordekaiser_unc_defendwraith>(),
        ModelDb.Card<Mordekaiser_unc_mortalbreak>(),
        ModelDb.Card<Mordekaiser_unc_wraithcritical>(),
        ModelDb.Card<Mordekaiser_unc_meteorwraithhammer>(),
        ModelDb.Card<Mordekaiser_unc_breakpowerhammer>(),
        
        //skill
        ModelDb.Card<Mordekaiser_unc_wraithpower>(),
        
        //attack
        ModelDb.Card<Mordekaiser_rare_maceofspades>(),
        ModelDb.Card<Mordekaiser_rare_creepingdeath>(),
        ModelDb.Card<Mordekaiser_rare_siphondestruction>(),
        ModelDb.Card<Mordekaiser_rare_fateend>(),
        ModelDb.Card<Mordekaiser_rare_quiesceecho>(),
        ModelDb.Card<Mordekaiser_rare_hellecho>(),
        ModelDb.Card<Mordekaiser_rare_souldrain>(),
        
    ];

}
