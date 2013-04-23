using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Items;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;
using Weaver.Test.Common.RelTypes;
using Weaver.Test.Common.Schema;
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
			
			WeaverTestUtil.CheckThrows<WeaverRelException>(true, () => {
				var c = r.Connection;
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ConnectionAlreadSet() {
			var r = new RootHasCandy(WeaverRelConn.OutToOne);

			WeaverTestUtil.CheckThrows<WeaverRelException>(true, () => {
				r.Connection = WeaverRelConn.OutToOneOrMore;
			});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FromNode() {
			var mockPath = new Mock<IWeaverPath>();
			var r = new RootHasCandy { Path = mockPath.Object };

			Root fromRoot = r.FromNode;
			Assert.NotNull(fromRoot, "FromNode should be filled.");
			mockPath.Verify(x => x.AddItem(It.IsAny<Root>()), Times.Once());

			Assert.True(fromRoot.IsFromNode, "Incorrect FromNode.IsFromNode.");
			Assert.AreEqual(!r.IsFromManyNodes, fromRoot.ExpectOneNode,
				"Incorrect FromNode.ExpectOneNode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ToNode() {
			var mockPath = new Mock<IWeaverPath>();
			var r = new RootHasCandy { Path = mockPath.Object };

			Candy toCandy = r.ToNode;
			Assert.NotNull(toCandy, "ToNode should be filled.");
			mockPath.Verify(x => x.AddItem(It.IsAny<Candy>()), Times.Once());

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
			var r = new RootHasCandy { Id = "abc-123" };
			Assert.AreEqual("RootHasCandy(Id='"+r.Id+"')", r.ItemIdentifier,
				"Incorrect ItemIdentifier.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverRelConn.InFromOne, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverRelConn.InFromZeroOrOne, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverRelConn.InFromOneOrMore, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverRelConn.InFromZeroOrMore, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverRelConn.OutToOne, "outE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverRelConn.OutToZeroOrOne, "outE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverRelConn.OutToOneOrMore, "outE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverRelConn.OutToZeroOrMore, "outE('"+TestSchema.RootHasPerson+"')")]
		public void BuildParameterizedString(WeaverRelConn pConn, string pExpectScript) {
			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);

			var r = new RootHasPerson() { Connection = pConn };
			r.Path = mockPath.Object;
			Assert.AreEqual(pExpectScript, r.BuildParameterizedString(), "Incorrect result.");
		}

	}

}