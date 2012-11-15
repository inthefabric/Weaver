using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Rels;

namespace Fabric.Test.Common.Nodes {

	/*================================================================================================*/
	public class TestCandy : TestNode, IQueryTestCandy {

		public bool IsChocolate { get; set; }
		public int Calories { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestCandy() {}

		/*--------------------------------------------------------------------------------------------*/
		public TestCandy(bool pIsRoot, bool pIsFromNode, bool pExpectOneNode) :
														base(pIsRoot, pIsFromNode, pExpectOneNode) {}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestRootHasPerson InRootHas {
			get { return new TestRootHasPerson(WeaverRelConn.InFromOneNode); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public TestPersonLikesCandy InPersonLikes {
			get { return new TestPersonLikesCandy(WeaverRelConn.InFromManyNodes); }
		}
		
	}

}