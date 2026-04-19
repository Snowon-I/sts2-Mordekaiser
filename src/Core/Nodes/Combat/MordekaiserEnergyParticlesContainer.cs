using Godot;
using Godot.Collections;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace Mordekaiser.Core.Nodes.Combat;

#if TOOLS
[Tool]
public partial class MordekaiserEnergyParticlesContainer : Node2D
{
	[Export(PropertyHint.None, "")]
	private Array<GpuParticles2D>? _particles;
	
		public void SetEmitting(bool emitting)
	{
		for (int i = 0; i < _particles.Count; i++)
		{
			_particles[i].Emitting = emitting;
		}
	}

	public void Restart()
	{
		for (int i = 0; i < _particles.Count; i++)
		{
			_particles[i].Restart();
		}
	}
	
}
#else
public partial class MordekaiserEnergyParticlesContainer : NParticlesContainer;
#endif
