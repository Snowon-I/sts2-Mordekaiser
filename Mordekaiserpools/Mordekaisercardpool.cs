using Godot;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.cards;

namespace Mordekaiser.Mordekaiserpools;

public class Mordekaisercardpool : CardPoolModel
{
    public override string Title => "mordekaiser";

    public override string EnergyColorName => "mordekaiser";

    public override string CardFrameMaterialPath => "card_frame_mordekaiser";

    public override Color DeckEntryCardColor => new ("00a1d6");

    public override Color EnergyOutlineColor => new ("045284FF");

    public override bool IsColorless => false;

    protected override CardModel[] GenerateAllCards() => [
        
        ModelDb.Card<Mordekaiser_ability_obliterate>(),
        ModelDb.Card<Mordekaiser_ability_deathsgrasp>(),
        ModelDb.Card<Mordekaiser_ability_indestructible_block>(),
        ModelDb.Card<Mordekaiser_ability_indestructible_live>(),
        ModelDb.Card<Mordekaiser_ability_realmofdeath>(),
        
        ModelDb.Card<Mordekaiser_base_strike>(),
        ModelDb.Card<Mordekaiser_base_defend>(),
        ModelDb.Card<Mordekaiser_base_darknessrise>(),
        ModelDb.Card<Mordekaiser_base_criticalattack>(),
        
        //com attack
        ModelDb.Card<Mordekaiser_com_strike>(),
        ModelDb.Card<Mordekaiser_com_quiescestrike>(),
        ModelDb.Card<Mordekaiser_com_dualhammerslam>(),
        ModelDb.Card<Mordekaiser_com_crushslam>(),
        ModelDb.Card<Mordekaiser_com_steelstrike>(),
        ModelDb.Card<Mordekaiser_com_underworldstrike>(),
        ModelDb.Card<Mordekaiser_com_agonytorment>(),
        ModelDb.Card<Mordekaiser_com_fleshreap>(),
        ModelDb.Card<Mordekaiser_com_unleashshadow>(),
        ModelDb.Card<Mordekaiser_com_steelcharge>(),
        ModelDb.Card<Mordekaiser_com_chargedhammerswing>(),
        
        //com skill
        ModelDb.Card<Mordekaiser_com_dashforward>(),
        ModelDb.Card<Mordekaiser_com_darksoulbarrier>(),
        ModelDb.Card<Mordekaiser_com_wraithfear>(),
        ModelDb.Card<Mordekaiser_com_prepareblock>(),
        ModelDb.Card<Mordekaiser_com_sacrifice>(),
        ModelDb.Card<Mordekaiser_com_darkdrain>(),
        ModelDb.Card<Mordekaiser_com_unmovingironbody>(),
        ModelDb.Card<Mordekaiser_com_greatquiesce>(),

        
        //unc attack
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
        
        //unc skill
        ModelDb.Card<Mordekaiser_unc_soulfollow>(),
        ModelDb.Card<Mordekaiser_unc_underworldempower>(),
        ModelDb.Card<Mordekaiser_unc_soulbodybarrier>(),
        ModelDb.Card<Mordekaiser_unc_shadowdrain>(),
        ModelDb.Card<Mordekaiser_unc_desperatestand>(),
        ModelDb.Card<Mordekaiser_unc_purifywraith>(),
        ModelDb.Card<Mordekaiser_unc_wraithpressure>(),
        ModelDb.Card<Mordekaiser_unc_wraithwarcry>(),
        ModelDb.Card<Mordekaiser_unc_wraithrecall>(),
        ModelDb.Card<Mordekaiser_unc_dashstance>(),
        ModelDb.Card<Mordekaiser_unc_shadowconvert>(),
        ModelDb.Card<Mordekaiser_unc_soulbarrier>(),
        ModelDb.Card<Mordekaiser_unc_wraithcycle>(),
        
        //unc power
        ModelDb.Card<Mordekaiser_unc_emberbladeinitiate>(),
        ModelDb.Card<Mordekaiser_unc_doomhour>(),
        ModelDb.Card<Mordekaiser_unc_soulcall>(),
        ModelDb.Card<Mordekaiser_unc_soulguarddarkdoom>(),
        ModelDb.Card<Mordekaiser_unc_solidguardsoulform>(),
        ModelDb.Card<Mordekaiser_unc_doomsoulendrelease>(),
        ModelDb.Card<Mordekaiser_unc_soulreleasedoom>(),
        ModelDb.Card<Mordekaiser_unc_soulpowerreave>(),
        ModelDb.Card<Mordekaiser_unc_ruindoom>(),
        ModelDb.Card<Mordekaiser_unc_soulforgebody>(),
        
        //rare attack
        ModelDb.Card<Mordekaiser_rare_maceofspades>(),
        ModelDb.Card<Mordekaiser_rare_creepingdeath>(),
        ModelDb.Card<Mordekaiser_rare_siphondestruction>(),
        ModelDb.Card<Mordekaiser_rare_fateend>(),
        ModelDb.Card<Mordekaiser_rare_quiesceecho>(),
        ModelDb.Card<Mordekaiser_rare_hellecho>(),
        ModelDb.Card<Mordekaiser_rare_souldrain>(),
        
        //rare skill
        ModelDb.Card<Mordekaiser_rare_underworldblessing>(),
        ModelDb.Card<Mordekaiser_rare_wraithstorm>(),
        ModelDb.Card<Mordekaiser_rare_wraithpower>(),
        ModelDb.Card<Mordekaiser_rare_underworldpact>(),
        ModelDb.Card<Mordekaiser_rare_doomimminent>(),
        ModelDb.Card<Mordekaiser_rare_unbreakable>(),
        ModelDb.Card<Mordekaiser_rare_collapse>(),
        ModelDb.Card<Mordekaiser_rare_finaljudgment>(),
        ModelDb.Card<Mordekaiser_rare_tearsoul>(),
        
        //rare power
        ModelDb.Card<Mordekaiser_rare_deathdominion>(),
        ModelDb.Card<Mordekaiser_rare_eternalimmortality>(),
        ModelDb.Card<Mordekaiser_rare_unyieldingpresence>(),
        ModelDb.Card<Mordekaiser_rare_absolutewill>(),
        ModelDb.Card<Mordekaiser_rare_soultome>(),
        ModelDb.Card<Mordekaiser_rare_soulsteelform>(),
        
    ];

}
