using NUnit.Framework;
using Weaver.Interfaces;
using Weaver.Items;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Fixtures.Items {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverRel {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverRelConn.InFromOne, "inE('RootHasCandy')[0]")]
		[TestCase(WeaverRelConn.InFromZeroOrOne, "inE('RootHasCandy')[0]")]
		[TestCase(WeaverRelConn.InFromOneOrMore, "inE('RootHasCandy')")]
		[TestCase(WeaverRelConn.InFromZeroOrMore, "inE('RootHasCandy')")]
		[TestCase(WeaverRelConn.OutToOne, "outE('RootHasCandy')[0]")]
		[TestCase(WeaverRelConn.OutToZeroOrOne, "outE('RootHasCandy')[0]")]
		[TestCase(WeaverRelConn.OutToOneOrMore, "outE('RootHasCandy')")]
		[TestCase(WeaverRelConn.OutToZeroOrMore, "outE('RootHasCandy')")]
		public void Gremlin(WeaverRelConn pConn, string pExpectGremlin) {
			IWeaverRel r = new RootHasCandy { Connection = pConn };

			Assert.AreEqual(pExpectGremlin, r.GremlinCode, "Incorrect GremlinCode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverRelConn.InFromOne, false, false, false)]
		[TestCase(WeaverRelConn.InFromZeroOrOne, false, false, false)]
		[TestCase(WeaverRelConn.InFromOneOrMore, true, false, false)]
		[TestCase(WeaverRelConn.InFromZeroOrMore, true, false, false)]
		[TestCase(WeaverRelConn.OutToOne, false, false, true)]
		[TestCase(WeaverRelConn.OutToZeroOrOne, false, false, true)]
		[TestCase(WeaverRelConn.OutToOneOrMore, false, true, true)]
		[TestCase(WeaverRelConn.OutToZeroOrMore, false, true, true)]
		public void Connection(WeaverRelConn pConn, bool pFromMany, bool pToMany, bool pOut) {
			IWeaverRel r = new RootHasCandy { Connection = pConn };

			Assert.AreEqual("RootHasCandy", r.Label, "Incorrect IsRoot.");
			Assert.AreEqual(pConn, r.Connection, "Incorrect Connection.");
			Assert.AreEqual(pFromMany, r.IsFromManyNodes, "Incorrect IsFromManyNodes.");
			Assert.AreEqual(pToMany, r.IsToManyNodes, "Incorrect IsToManyNodes.");
			Assert.AreEqual(pOut, r.IsOutgoing, "Incorrect IsOutgoing.");
		}

	}

}