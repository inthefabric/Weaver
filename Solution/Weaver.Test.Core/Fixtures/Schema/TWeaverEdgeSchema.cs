using NUnit.Framework;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Schema;
using Weaver.Test.Core.Utils;

namespace Weaver.Test.Core.Fixtures.Schema {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverEdgeSchema : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Constructor() {
			var outVert = new WeaverVertexSchema("FROM", "F");
			var inVert = new WeaverVertexSchema("TO", "T");
			var rs = new WeaverEdgeSchema(outVert, "FromLikesTo", "FLT", "Likes", inVert);

			Assert.AreEqual(outVert, rs.OutVertex, "Incorrect FromNode.");
			Assert.AreEqual("FromLikesTo", rs.Name, "Incorrect Name.");
			Assert.AreEqual(inVert, rs.InVertex, "Incorrect ToNode.");
			Assert.AreEqual(WeaverEdgeConn.OutToZeroOrMore, rs.OutVertexConn, "Incorrect FromNodeComm.");
			Assert.AreEqual(WeaverEdgeConn.InFromZeroOrMore, rs.InVertexConn, "Incorrect ToNodeComm.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void OutVertexConnFail() {
			var rs = new WeaverEdgeSchema(null, "TestDoesPass", "TDP", "Does", null);

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => rs.OutVertexConn = WeaverEdgeConn.InFromOne
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void InVertexConnFail() {
			var rs = new WeaverEdgeSchema(null, "TestDoesPass", "TDP", "Does", null);

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => rs.InVertexConn = WeaverEdgeConn.OutToOne
			);
		}

	}

}