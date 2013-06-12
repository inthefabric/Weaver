using Moq;
using NUnit.Framework;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Test.Core.Common.EdgeTypes;
using Weaver.Test.Core.Common.Edges;
using Weaver.Test.Core.Common.Schema;
using Weaver.Test.Core.Common.Vertices;
using Weaver.Test.Core.Utils;

namespace Weaver.Test.Core.Fixtures.Elements {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverEdge : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverEdgeConn.InFromOne, false, false, false)]
		[TestCase(WeaverEdgeConn.InFromZeroOrOne, false, false, false)]
		[TestCase(WeaverEdgeConn.InFromOneOrMore, true, false, false)]
		[TestCase(WeaverEdgeConn.InFromZeroOrMore, true, false, false)]
		[TestCase(WeaverEdgeConn.OutToOne, false, false, true)]
		[TestCase(WeaverEdgeConn.OutToZeroOrOne, false, false, true)]
		[TestCase(WeaverEdgeConn.OutToOneOrMore, false, true, true)]
		[TestCase(WeaverEdgeConn.OutToZeroOrMore, false, true, true)]
		public void Connection(WeaverEdgeConn pConn, bool pFromMany, bool pToMany, bool pOut) {
			var r = new RootHasCandy { Connection = pConn };

			Assert.AreEqual("RootHasCandy", r.Label, "Incorrect IsRoot.");
			Assert.AreEqual(pConn, r.Connection, "Incorrect Connection.");
			Assert.AreEqual(pFromMany, r.IsFromManyVertices, "Incorrect IsFromManyVertexs.");
			Assert.AreEqual(pToMany, r.IsToManyVertices, "Incorrect IsToManyVertexs.");
			Assert.AreEqual(pOut, r.IsOutgoing, "Incorrect IsOutgoing.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ConnectionNotSet() {
			var r = new RootHasCandy();
			
			WeaverTestUtil.CheckThrows<WeaverEdgeException>(true, () => {
				var c = r.Connection;
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ConnectionAlreadSet() {
			var r = new RootHasCandy(WeaverEdgeConn.OutToOne);

			WeaverTestUtil.CheckThrows<WeaverEdgeException>(true, () => {
				r.Connection = WeaverEdgeConn.OutToOneOrMore;
			});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FromVertex() {
			var mockPath = new Mock<IWeaverPath>();
			var r = new RootHasCandy { Path = mockPath.Object };

			Root fromRoot = r.OutVertex;
			Assert.NotNull(fromRoot, "FromVertex should be filled.");
			mockPath.Verify(x => x.AddItem(It.IsAny<Root>()), Times.Once());

			Assert.True(fromRoot.IsFromVertex, "Incorrect FromVertex.IsFromVertex.");
			Assert.AreEqual(!r.IsFromManyVertices, fromRoot.ExpectOneVertex,
				"Incorrect FromVertex.ExpectOneVertex.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ToVertex() {
			var mockPath = new Mock<IWeaverPath>();
			var r = new RootHasCandy { Path = mockPath.Object };

			Candy toCandy = r.InVertex;
			Assert.NotNull(toCandy, "ToVertex should be filled.");
			mockPath.Verify(x => x.AddItem(It.IsAny<Candy>()), Times.Once());

			Assert.False(toCandy.IsFromVertex, "Incorrect ToVertex.IsFromVertex.");
			Assert.AreEqual(!r.IsToManyVertices, toCandy.ExpectOneVertex,
				"Incorrect ToVertex.ExpectOneVertex.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void VertexType() {
			var r = new PersonLikesCandy();

			Assert.AreEqual(typeof(Person), r.OutVertexType, "Incorrect FromVertexType.");
			Assert.AreEqual(typeof(Candy), r.InVertexType, "Incorrect ToVertexType.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void EdgeType() {
			var r = new PersonLikesCandy();
			IWeaverEdgeType hasType = r.EdgeType;

			Assert.NotNull(hasType, "EdgeType should be filled.");
			Assert.NotNull((hasType as Likes), "Incorrect EdgeType.");
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
		[TestCase(WeaverEdgeConn.InFromOne, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.InFromZeroOrOne, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.InFromOneOrMore, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.InFromZeroOrMore, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.OutToOne, "outE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.OutToZeroOrOne, "outE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.OutToOneOrMore, "outE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.OutToZeroOrMore, "outE('"+TestSchema.RootHasPerson+"')")]
		public void BuildParameterizedString(WeaverEdgeConn pConn, string pExpectScript) {
			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);

			var r = new RootHasPerson() { Connection = pConn };
			r.Path = mockPath.Object;
			Assert.AreEqual(pExpectScript, r.BuildParameterizedString(), "Incorrect result.");
		}

	}

}