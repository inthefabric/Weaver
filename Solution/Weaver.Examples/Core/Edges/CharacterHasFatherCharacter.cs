using Weaver.Core.Elements;
using Weaver.Examples.Core.EdgeTypes;
using Weaver.Examples.Core.Vertices;

namespace Weaver.Examples.Core.Edges {

	/*================================================================================================*/
	[WeaverEdge("father", typeof(Character), typeof(Character))]
	public class CharacterHasFatherCharacter : WeaverEdge<Character, HasFather, Character> {
		
	}

}