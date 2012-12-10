using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Items;
using Weaver.Test.Common;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;
using Weaver.Test.Common.RelTypes;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Items {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverRel : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
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
			var r = new RootHasCandy { Connection = pConn };

			Assert.AreEqual("RootHasCandy", r.Label, "Incorrect IsRoot.");
			Assert.AreEqual(pConn, r.Connection, "Incorrect Connection.");
			Assert.AreEqual(pFromMany, r.IsFromManyNodes, "Incorrect IsFromManyNodes.");
			Assert.AreEqual(pToMany, r.IsToManyNodes, "Incorrect IsToManyNodes.");
			Assert.AreEqual(pOut, r.IsOutgoing, "Incorrect IsOutgoing.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ConnectionNotSet() {
			var r = new RootHasCandy();
			
			WeaverTestUtils.CheckThrows<WeaverRelException>(true, () => {
				var c = r.Connection;
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ConnectionAlreadSet() {
			var r = new RootHasCandy(WeaverRelConn.OutToOne);

			WeaverTestUtils.CheckThrows<WeaverRelException>(true, () => {
				r.Connection = WeaverRelConn.OutToOneOrMore;
			});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FromNode() {
			var p = new TestPath();
			var r = new RootHasCandy { Path = p };
			int len = p.Length;

			Root fromRoot = r.FromNode;

			Assert.NotNull(fromRoot, "FromNode should be filled.");
			Assert.AreEqual(p, fromRoot.Path, "Incorrect FromNode.Path.");
			Assert.AreEqual(len+1, p.Length, "Incorrect Path.Length.");

			Assert.True(fromRoot.IsFromNode, "Incorrect FromNode.IsFromNode.");
			Assert.AreEqual(!r.IsFromManyNodes, fromRoot.ExpectOneNode,
				"Incorrect FromNode.ExpectOneNode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ToNode() {
			var p = new TestPath();
			var r = new RootHasCandy { Path = p };
			int len = p.Length;

			Candy toCandy = r.ToNode;

			Assert.NotNull(toCandy, "ToNode should be filled.");
			Assert.AreEqual(p, toCandy.Path, "Incorrect ToNode.Path.");
			Assert.AreEqual(len+1, p.Length, "Incorrect Path.Length.");

			Assert.False(toCandy.IsFromNode, "Incorrect ToNode.IsFromNode.");
			Assert.AreEqual(!r.IsToManyNodes, toCandy.ExpectOneNode,
				"Incorrect ToNode.ExpectOneNode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NodeType() {
			var r = new PersonLikesCandy();

			Assert.AreEqual(typeof(Person), r.FromNodeType, "Incorrect FromNodeType.");
			Assert.AreEqual(typeof(Candy), r.ToNodeType, "Incorrect ToNodeType.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void RelType() {
			var r = new PersonLikesCandy();
			IWeaverRelType hasType = r.RelType;

			Assert.NotNull(hasType, "RelType should be filled.");
			Assert.NotNull((hasType as Likes), "Incorrect RelType.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Label() {
			var r = new RootHasCandy();
			Assert.AreEqual("RootHasCandy", r.Label, "Incorrect Label.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ItemIdentifier() {
			var r = new RootHasCandy { Id = 123 };
			Assert.AreEqual("RootHasCandy(Id=123)", r.ItemIdentifier, "Incorrect ItemIdentifier.");
		}


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
			var r = new RootHasCandy { Connection = pConn };

			Assert.AreEqual(pExpectGremlin, r.GremlinCode, "Incorrect GremlinCode.");
		}

	}

}