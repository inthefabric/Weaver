using Weaver.Core.Elements;
using Weaver.Examples.Core.Edges;

namespace Weaver.Examples.Core.Vertices {

	/*================================================================================================*/
	[WeaverVertex]
	public class Location : BaseVertex {
	
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CharacterLivesLocation InCharacterLives {
			get { return NewEdge<CharacterLivesLocation>(WeaverEdgeConn.InZeroOrMore); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public MonsterLivesLocation InMonsterLives {
			get { return NewEdge<MonsterLivesLocation>(WeaverEdgeConn.InZeroOrMore); }
		}

	}

}