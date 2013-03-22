using NUnit.Framework;
using Weaver.Functions;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncRemoveEach : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			var f = new WeaverFuncRemoveEach<Person>();
			Assert.AreEqual("remove()", f.BuildParameterizedString(), "Incorrect result.");
		}

	}

}