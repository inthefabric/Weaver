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
			var fromNode = new WeaverVertexSchema("FROM", "F");
			var toNode = new WeaverVertexSchema("TO", "T");
			var rs = new WeaverEdgeSchema(fromNode, "FromLikesTo", "FLT", "Likes", toNode);

			Assert.AreEqual(fromNode, rs.FromNode, "Incorrect FromNode.");
			Assert.AreEqual("FromLikesTo", rs.Name, "Incorrect Name.");
			Assert.AreEqual(toNode, rs.ToNode, "Incorrect ToNode.");
			Assert.AreEqual(WeaverEdgeConn.OutToZeroOrMore, rs.FromNodeConn, "Incorrect FromNodeComm.");
			Assert.AreEqual(WeaverEdgeConn.InFromZeroOrMore, rs.ToNodeConn, "Incorrect ToNodeComm.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FromNodeConnFail() {
			var rs = new WeaverEdgeSchema(null, "TestDoesPass", "TDP", "Does", null);

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => rs.FromNodeConn = WeaverEdgeConn.InFromOne
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ToNodeConnFail() {
			var rs = new WeaverEdgeSchema(null, "TestDoesPass", "TDP", "Does", null);

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => rs.ToNodeConn = WeaverEdgeConn.OutToOne
			);
		}

	}

}