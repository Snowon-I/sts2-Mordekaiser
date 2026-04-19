using Godot;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

namespace Mordekaiser.Core.CardLibrary;

#if TOOLS
public partial class MordekaiserPoolFilter : Control
{
	[Export] private float ignore_drag_threshold = -1.0f;
}
#else
public partial class MordekaiserPoolFilter : NCardPoolFilter;
#endif
