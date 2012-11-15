using NUnit.Framework;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures.Items {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(false, false, false, "inV")]
		[TestCase(false, false, true, "inV(0)")]
		[TestCase(false, true, false, "outV")]
		[TestCase(false, true, true, "outV(0)")]
		[TestCase(true, false, false, "v(0)")]
		[TestCase(true, false, true, "v(0)")]
		[TestCase(true, true, false, "v(0)")]
		[TestCase(true, true, true, "v(0)")]
		public void Gremlin(bool pIsRoot, bool pIsFrom, bool pExpectOne, string pExpectGremlin) {
			var n = new Person(pIsRoot, pIsFrom, pExpectOne);

			Assert.AreEqual(pIsRoot, n.IsRoot, "Incorrect IsRoot.");
			Assert.AreEqual(pIsFrom, n.IsFromNode, "Incorrect IsFromNode.");
			Assert.AreEqual(pExpectOne, n.ExpectOneNode, "Incorrect IsFromNode.");
			Assert.AreEqual(pExpectGremlin, n.GremlinCode, "Incorrect GremlinCode.");
		}

	}

}