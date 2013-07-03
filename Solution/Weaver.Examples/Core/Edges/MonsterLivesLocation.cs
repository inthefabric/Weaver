using Weaver.Core.Elements;
using Weaver.Examples.Core.Vertices;

namespace Weaver.Examples.Core.Edges {

	/*================================================================================================*/
	[WeaverEdge("lives", typeof(Monster), typeof(Location))]
	public class MonsterLivesLocation : LivesLocation<Monster> {
		
	}

}