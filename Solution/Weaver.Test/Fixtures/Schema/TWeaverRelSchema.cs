using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Items;
using Weaver.Schema;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Schema {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverRelSchema : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Constructor() {
			var fromNode = new WeaverNodeSchema("FROM", "F");
			var toNode = new WeaverNodeSchema("TO", "T");
			var rs = new WeaverRelSchema(fromNode, "FromLikesTo", "FLT", "Likes", toNode);

			Assert.AreEqual(fromNode, rs.FromNode, "Incorrect FromNode.");
			Assert.AreEqual("FromLikesTo", rs.Name, "Incorrect Name.");
			Assert.AreEqual(toNode, rs.ToNode, "Incorrect ToNode.");
			Assert.AreEqual(WeaverRelConn.OutToZeroOrMore, rs.FromNodeConn, "Incorrect FromNodeComm.");
			Assert.AreEqual(WeaverRelConn.InFromZeroOrMore, rs.ToNodeConn, "Incorrect ToNodeComm.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FromNodeConnFail() {
			var rs = new WeaverRelSchema(null, "TestDoesPass", "TDP", "Does", null);

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => rs.FromNodeConn = WeaverRelConn.InFromOne
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ToNodeConnFail() {
			var rs = new WeaverRelSchema(null, "TestDoesPass", "TDP", "Does", null);

			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => rs.ToNodeConn = WeaverRelConn.OutToOne
			);
		}

	}

}