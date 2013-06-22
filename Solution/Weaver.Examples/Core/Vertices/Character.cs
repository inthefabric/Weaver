using Weaver.Core.Elements;

namespace Weaver.Examples.Core.Vertices {

	/*================================================================================================*/
	public class Character : BaseVertex {

		[WeaverProperty("age")]
		public string Age { get; set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CharacterHasFatherCharacter OutHasFatherCharacter {
			get { return NewEdge<CharacterHasFatherCharacter>(WeaverEdgeConn.OutZeroOrOne); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public CharacterHasMotherCharacter OutHasMotherCharacter {
			get { return NewEdge<CharacterHasMotherCharacter>(WeaverEdgeConn.OutZeroOrOne); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public CharacterHasBrotherCharacter OutHasBrotherCharacter {
			get { return NewEdge<CharacterHasBrotherCharacter>(WeaverEdgeConn.OutZeroOrOne); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public CharacterHasPetMonster OutHasPetMonster {
			get { return NewEdge<CharacterHasPetMonster>(WeaverEdgeConn.OutZeroOrOne); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public CharacterBattledMonster OutBattledMonster {
			get { return NewEdge<CharacterBattledMonster>(WeaverEdgeConn.OutZeroOrMore); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public CharacterLivesLocation OutLivesLocation {
			get { return NewEdge<CharacterLivesLocation>(WeaverEdgeConn.OutZeroOrOne); }
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CharacterHasFatherCharacter InCharacterHasFather {
			get { return NewEdge<CharacterHasFatherCharacter>(WeaverEdgeConn.InZeroOrOne); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public CharacterHasMotherCharacter InCharacterHasMother {
			get { return NewEdge<CharacterHasMotherCharacter>(WeaverEdgeConn.InZeroOrOne); }
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public CharacterHasBrotherCharacter InCharacterHasBrother {
			get { return NewEdge<CharacterHasBrotherCharacter>(WeaverEdgeConn.InZeroOrOne); }
		}
		
	}

}