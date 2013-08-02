using System;
using Weaver.Core;
using Weaver.Examples.Core.Edges;
using Weaver.Examples.Core.Vertices;

namespace Weaver.Examples.Core {

	/*================================================================================================*/
	public static class Weave {
	
		public static Type[] VertexTypes = new [] {
			typeof(Demigod),
			typeof(God),
			typeof(Human),
			typeof(Location),
			typeof(Monster),
			typeof(Titan)
		};
		
		public static Type[] EdgeTypes = new [] {
			typeof(CharacterBattledMonster),
			typeof(CharacterHasBrotherCharacter),
			typeof(CharacterHasFatherCharacter),
			typeof(CharacterHasMotherCharacter),
			typeof(CharacterHasPetMonster),
			typeof(CharacterLivesLocation),
			typeof(MonsterLivesLocation)
		};
		
		public static WeaverInstance Inst = new WeaverInstance(VertexTypes, EdgeTypes);
				
	}

}