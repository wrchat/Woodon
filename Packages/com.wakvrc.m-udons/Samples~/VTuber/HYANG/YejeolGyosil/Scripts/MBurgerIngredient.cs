using UdonSharp;
using WakVRC;

namespace Mascari4615.Project.VTuber.HYANG.YejolGyosil
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
	public class MBurgerIngredient : MPickup
	{
		public int Index { get; private set; }

		public void Init(int index)
		{
			Index = index;
		}
	}
}