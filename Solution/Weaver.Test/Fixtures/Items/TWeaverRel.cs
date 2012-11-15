using Fabric.Domain.Graph.Interfaces;
using Fabric.Domain.Graph.Items;
using Fabric.Test.Common.Rels;
using NUnit.Framework;

namespace Fabric.Test.Fixtures.Items {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverRel {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverRelConn.InFromOneNode, "inE('RootHasCandy')[0]")]
		[TestCase(WeaverRelConn.InFromManyNodes, "inE('RootHasCandy')")]
		[TestCase(WeaverRelConn.OutToOneNode, "outE('RootHasCandy')[0]")]
		[TestCase(WeaverRelConn.OutToManyNodes, "outE('RootHasCandy')")]
		public void Gremlin(WeaverRelConn pConn, string pExpectGremlin) {
			IWeaverRel r = new TestRootHasCandy(pConn);

			/*Assert.AreEqual("RootHasCandy", r.Label, "Incorrect IsRoot.");
			Assert.AreEqual(pIsFromList, r.FromManyNodes, "Incorrect IsRoot.");
			Assert.AreEqual(pIsRelOut, r.IsRelOut, "Incorrect IsRoot.");
			Assert.AreEqual(pIsToList, r.ToManyNodes, "Incorrect IsFromNode.");*/
			Assert.AreEqual(pExpectGremlin, r.GremlinCode, "Incorrect GremlinCode.");
		}

	}

}