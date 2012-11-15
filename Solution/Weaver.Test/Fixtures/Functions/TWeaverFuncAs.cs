using Fabric.Domain.Graph.Functions;
using Fabric.Test.Common.Nodes;
using NUnit.Framework;

namespace Fabric.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncAs {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("a", "as('a')")]
		[TestCase("reallyLongLabelText", "as('reallyLongLabelText')")]
		public void Gremlin(string pLabel, string pExpectGremlin) {
			var f = new WeaverFuncAs<Person>(new Person(), pLabel);

			Assert.AreEqual(pLabel, f.Label, "Incorrect Label.");
			Assert.AreEqual(pExpectGremlin, f.GremlinCode, "Incorrect GremlinCode.");
		}

	}

}