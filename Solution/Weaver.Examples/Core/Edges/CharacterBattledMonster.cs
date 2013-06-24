using Weaver.Core.Elements;
using Weaver.Examples.Core.EdgeTypes;
using Weaver.Examples.Core.Vertices;

namespace Weaver.Examples.Core.Vertices {

	/*================================================================================================*/
	[WeaverEdge("battled", typeof(Character), typeof(Monster))]
	public class CharacterBattledMonster : WeaverEdge<Character, Battled, Monster> {
		
		[WeaverProperty("time")]
		public long Time { get; set; }
		
		[WeaverProperty("place")]
		public float[] Place { get; set; }
		
	}

}