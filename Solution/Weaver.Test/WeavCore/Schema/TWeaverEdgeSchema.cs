using NUnit.Framework;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Schema;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Schema {

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
			Assert.AreEqual(WeaverEdgeConn.OutZeroOrMore, rs.OutVertexConn, "Incorrect FromNodeComm.");
			Assert.AreEqual(WeaverEdgeConn.InZeroOrMore, rs.InVertexConn, "Incorrect ToNodeComm.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void OutVertexConnFail() {
			var rs = new WeaverEdgeSchema(null, "TestDoesPass", "TDP", "Does", null);

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => rs.OutVertexConn = WeaverEdgeConn.InOne
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void InVertexConnFail() {
			var rs = new WeaverEdgeSchema(null, "TestDoesPass", "TDP", "Does", null);

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => rs.InVertexConn = WeaverEdgeConn.OutOne
			);
		}

	}

}