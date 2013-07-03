using Weaver.Core.Elements;
using Weaver.Examples.Core.Vertices;

namespace Weaver.Examples.Core.Edges {

	/*================================================================================================*/
	[WeaverEdge("lives", typeof(Character), typeof(Location))]
	public class CharacterLivesLocation : LivesLocation<Character> {
		
	}

}