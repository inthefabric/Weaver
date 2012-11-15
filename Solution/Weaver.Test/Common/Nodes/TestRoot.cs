using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Rels;

namespace Fabric.Test.Common.Nodes {

	/*================================================================================================*/
	public class TestRoot : TestNode, IQueryTestRoot {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestRoot() { }

		/*--------------------------------------------------------------------------------------------*/
		public TestRoot(bool pIsFromNode, bool pExpectOneNode) :
															base(true, pIsFromNode, pExpectOneNode) {}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestRootHasCandy OutHasCandy {
			get { return new TestRootHasCandy(WeaverRelConn.OutToManyNodes); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public TestRootHasPerson OutHasPerson {
			get { return new TestRootHasPerson(WeaverRelConn.OutToManyNodes); }
		}

	}

}