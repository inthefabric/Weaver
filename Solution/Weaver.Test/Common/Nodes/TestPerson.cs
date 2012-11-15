using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Rels;

namespace Fabric.Test.Common.Nodes {

	/*================================================================================================*/
	public class TestPerson : TestNode, IQueryTestPerson {

		public bool IsMale { get; set; }
		public float Age { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestPerson() { }

		/*--------------------------------------------------------------------------------------------*/
		public TestPerson(bool pIsRoot, bool pIsFromNode, bool pExpectOneNode) :
			base(pIsRoot, pIsFromNode, pExpectOneNode) { }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestPersonLikesCandy OutLikesCandy {
			get { return new TestPersonLikesCandy(WeaverRelConn.OutToManyNodes); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public TestPersonKnowsPerson OutKnowsPerson {
			get { return new TestPersonKnowsPerson(WeaverRelConn.OutToManyNodes); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestRootHasPerson InRootHas {
			get { return new TestRootHasPerson(WeaverRelConn.InFromOneNode); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public TestPersonKnowsPerson InPersonKnows {
			get { return new TestPersonKnowsPerson(WeaverRelConn.InFromManyNodes); }
		}

	}

}