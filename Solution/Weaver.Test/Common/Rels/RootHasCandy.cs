using Weaver.Items;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.RelTypes;

namespace Weaver.Test.Common.Rels {

	/*================================================================================================*/
	public class RootHasCandy : WeaverRel<Root, Has, Candy> {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy() {}

		/*--------------------------------------------------------------------------------------------*/
		public RootHasCandy(WeaverRelConn pConn) : base(pConn) {}

	}

}