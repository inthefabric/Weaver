using Weaver.Core.Elements;
using Weaver.Examples.Core.Edges;

namespace Weaver.Examples.Core.Vertices {

	/*================================================================================================*/
	[WeaverVertex]
	public class Monster : BaseVertex {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public MonsterLivesLocation OutLivesLocation {
			get { return NewEdge<MonsterLivesLocation>(WeaverEdgeConn.OutZeroOrOne); }
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CharacterBattledMonster InCharacterBattled {
			get { return NewEdge<CharacterBattledMonster>(WeaverEdgeConn.InZeroOrOne); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public CharacterHasPetMonster InCharacterHasPet {
			get { return NewEdge<CharacterHasPetMonster>(WeaverEdgeConn.InZeroOrOne); }
		}
		
	}

}