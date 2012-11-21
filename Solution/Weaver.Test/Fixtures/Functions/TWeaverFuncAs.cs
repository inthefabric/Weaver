using Moq;
using NUnit.Framework;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncAs {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(2)]
		[TestCase(33)]
		[TestCase(444)]
		public void Gremlin(int pPathLen) {
			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Length).Returns(pPathLen);
			int itemI = pPathLen-1;

			var f = new WeaverFuncAs<Person>(mockPath.Object);

			Assert.AreEqual("step"+itemI, f.Label, "Incorrect Label.");
			Assert.AreEqual("as('step"+itemI+"')", f.GremlinCode, "Incorrect GremlinCode.");
		}

	}

}