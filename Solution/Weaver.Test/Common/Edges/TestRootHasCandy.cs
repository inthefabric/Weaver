using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Nodes;
using Fabric.Test.Common.RelTypes;

namespace Fabric.Test.Common.Rels {

	/*================================================================================================*/
	public class TestRootHasCandy : 
							WeaverRel<IQueryTestRoot, TestRoot, TestHas, IQueryTestCandy, TestCandy> {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestRootHasCandy(WeaverRelConn pConn) : base(pConn) {}

		/*--------------------------------------------------------------------------------------------*/
		public override string Label { get { return "RootHasCandy"; } }

	}

}