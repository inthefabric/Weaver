﻿using Weaver.Core.Elements;
using Weaver.Examples.Core.EdgeTypes;
using Weaver.Examples.Core.Vertices;

namespace Weaver.Examples.Core.Vertices {

	/*================================================================================================*/
	[WeaverEdge("pet", typeof(Character), typeof(Monster))]
	public class CharacterHasPetMonster : WeaverEdge<Character, HasPet, Monster> {
		
	}

}