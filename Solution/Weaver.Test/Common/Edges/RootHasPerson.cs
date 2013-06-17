using Weaver.Core.Elements;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Vertices;

namespace Weaver.Test.Common.Edges {

	/*================================================================================================*/
	public class RootHasPerson : WeaverEdge<RootHasPerson, Root, Has, Person> {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override Root BuildOutVertex () {
			return new Root();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		protected override Person BuildInVertex () {
			return new Person();
		}
		
	}

}