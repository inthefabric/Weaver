using NUnit.Framework;
using Weaver.Core.Schema;

namespace Weaver.Test.Core.Fixtures.Schema {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverVertexSchema : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Constructor() {
			var ns = new WeaverVertexSchema("Person", "P");
			
			Assert.AreEqual("Person", ns.Name, "Incorrect Name.");
			Assert.AreEqual("P", ns.DbName, "Incorrect Short.");
			Assert.NotNull(ns.Props, "Props should not be null.");

			Assert.Null(ns.BaseVertex, "BaseNode should be null by default.");
			Assert.False(ns.IsAbstract, "Incorrect default IsAbstract.");
			Assert.False(ns.IsRoot, "Incorrect default IsRoot.");
		}

	}

}