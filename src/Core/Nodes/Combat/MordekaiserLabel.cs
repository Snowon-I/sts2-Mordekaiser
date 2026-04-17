using MegaCrit.Sts2.addons.mega_text;

namespace Mordekaiser.Core.Nodes.Combat;

#if TOOLS
[Tool]
public partial class MordekaiserLabel : Label
{
	[Export] public bool AutoSizeEnabled { get; set; } = true;
	[Export] public int MinFontSize { get; set; } = 8;
	[Export] public int MaxFontSize { get; set; } = 100;
}
#else 
public partial class MordekaiserLabel : MegaLabel;
#endif
