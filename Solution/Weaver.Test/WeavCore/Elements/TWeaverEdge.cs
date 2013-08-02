using System;
using Moq;
using NUnit.Framework;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Test.Common.EdgeTypes;
using Weaver.Test.Common.Edges;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Elements {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverEdge : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverEdgeConn.InOne, false, false, false)]
		[TestCase(WeaverEdgeConn.InZeroOrOne, false, false, false)]
		[TestCase(WeaverEdgeConn.InOneOrMore, true, false, false)]
		[TestCase(WeaverEdgeConn.InZeroOrMore, true, false, false)]
		[TestCase(WeaverEdgeConn.OutOne, false, false, true)]
		[TestCase(WeaverEdgeConn.OutZeroOrOne, false, false, true)]
		[TestCase(WeaverEdgeConn.OutOneOrMore, false, true, true)]
		[TestCase(WeaverEdgeConn.OutZeroOrMore, false, true, true)]
		public void Connection(WeaverEdgeConn pConn, bool pFromMany, bool pToMany, bool pOut) {
			var r = new RootHasCandy { Connection = pConn };

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
			var r = new RootHasCandy(WeaverEdgeConn.OutOne);

			WeaverTestUtil.CheckThrows<WeaverEdgeException>(true, () => {
				r.Connection = WeaverEdgeConn.OutOneOrMore;
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
		[TestCase(true)]
		[TestCase(false)]
		public void IsValidOutVertexType(bool pValid) {
			Type t = (pValid ? typeof(Person) : typeof(Root));
			var plc = new PersonLikesCandy();
			Assert.AreEqual(pValid, plc.IsValidOutVertexType(t), "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void IsValidInVertexType(bool pValid) {
			Type t = (pValid ? typeof(Candy) : typeof(Person));
			var plc = new PersonLikesCandy();
			Assert.AreEqual(pValid, plc.IsValidInVertexType(t), "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
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
		public void ItemIdentifier() {
			var r = new RootHasCandy { Id = "abc-123" };
			Assert.AreEqual("RootHasCandy(Id='"+r.Id+"')", r.ItemIdentifier,
				"Incorrect ItemIdentifier.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverEdgeConn.InOne, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.InZeroOrOne, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.InOneOrMore, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.InZeroOrMore, "inE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.OutOne, "outE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.OutZeroOrOne, "outE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.OutOneOrMore, "outE('"+TestSchema.RootHasPerson+"')")]
		[TestCase(WeaverEdgeConn.OutZeroOrMore, "outE('"+TestSchema.RootHasPerson+"')")]
		public void BuildParameterizedString(WeaverEdgeConn pConn, string pExpectScript) {
			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);

			var r = new RootHasPerson() { Connection = pConn };
			r.Path = mockPath.Object;
			Assert.AreEqual(pExpectScript, r.BuildParameterizedString(), "Incorrect result.");
		}

	}

}