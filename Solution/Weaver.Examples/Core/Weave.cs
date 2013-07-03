using Weaver.Core.Elements;
using Weaver.Examples.Core.EdgeTypes;
using Weaver.Examples.Core.Edges;
using Weaver.Examples.Core.Vertices;
using Weaver.Core;
using System;

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