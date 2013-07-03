using Weaver.Core.Elements;
using Weaver.Examples.Core.EdgeTypes;
using Weaver.Examples.Core.Vertices;

namespace Weaver.Examples.Core.Edges {

	/*================================================================================================*/
	[WeaverEdge("brother", typeof(Character), typeof(Character))]
	public class CharacterHasBrotherCharacter : WeaverEdge<Character, HasBrother, Character> {
		
	}

}