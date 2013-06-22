using Weaver.Core.Elements;
using Weaver.Examples.Core.EdgeTypes;
using Weaver.Examples.Core.Vertices;

namespace Weaver.Examples.Core.Vertices {

	/*================================================================================================*/
	[WeaverEdge("mother", typeof(Character), typeof(Character))]
	public class CharacterHasMotherCharacter : WeaverEdge<Character, HasMother, Character> {
		
	}

}