using NUnit.Framework;
using Weaver.Schema;

namespace Weaver.Test.Fixtures.Schema {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverNodeSchema : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Constructor() {
			var ns = new WeaverNodeSchema("Person", "P");
			
			Assert.AreEqual("Person", ns.Name, "Incorrect Name.");
			Assert.AreEqual("P", ns.Short, "Incorrect Short.");
			Assert.NotNull(ns.Props, "Props should not be null.");

			Assert.Null(ns.BaseNode, "BaseNode should be null by default.");
			Assert.False(ns.IsAbstract, "Incorrect default IsAbstract.");
			Assert.False(ns.IsRoot, "Incorrect default IsRoot.");
		}

	}

}