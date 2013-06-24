using Weaver.Core.Elements;
using Weaver.Examples.Core.EdgeTypes;
using Weaver.Examples.Core.Vertices;
using Weaver.Core.Query;
using Weaver.Core.Graph;
using Weaver.Core.Pipe;

namespace Weaver.Examples.Core {

	/*================================================================================================*/
	public static class Traversals {
	
		//Demonstrating some traversals shown here:
		//https://github.com/thinkaurelius/titan/wiki/Getting-Started
		
		//See unit tests in Weaver.Test.WeavExamples.TTraversal
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery GetCharacterByName<T>(string pName, string pVarName,
										out IWeaverVarAlias<T> pCharVar) where T : Character, new() {
			IWeaverQuery q = Weave.Inst.Graph
				.V.ExactIndex<T>(x => x.Name, pName).Next()
				.ToQuery();
			
			return WeaverQuery.StoreResultAsVar(pVarName, q, out pCharVar);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static Character GetGrandchildOfCharacter(IWeaverVarAlias<Character> pCharVar) {
			return Weave.Inst.FromVar(pCharVar)
				.InCharacterHasFather.OutVertex
				.InCharacterHasFather.OutVertex;
		}
				
	}

}