using Weaver.Core.Elements;
using Weaver.Examples.Core.EdgeTypes;
using Weaver.Examples.Core.Vertices;

namespace Weaver.Examples.Core.Vertices {

	/*================================================================================================*/
	[WeaverEdge("lives", typeof(Monster), typeof(Location))]
	public class MonsterLivesLocation : LivesLocation<Monster> {
		
	}

}