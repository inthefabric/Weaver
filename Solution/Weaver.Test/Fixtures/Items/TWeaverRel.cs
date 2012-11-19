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

			/*Assert.AreEqual("RootHasCandy", r.Label, "Incorrect IsRoot.");
			Assert.AreEqual(pIsFromList, r.FromManyNodes, "Incorrect IsRoot.");
			Assert.AreEqual(pIsRelOut, r.IsRelOut, "Incorrect IsRoot.");
			Assert.AreEqual(pIsToList, r.ToManyNodes, "Incorrect IsFromNode.");*/
			Assert.AreEqual(pExpectGremlin, r.GremlinCode, "Incorrect GremlinCode.");
		}

	}

}