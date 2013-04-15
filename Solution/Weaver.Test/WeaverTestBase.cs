using NUnit.Framework;
using Weaver.Test.Common.Schema;

namespace Weaver.Test {

	/*================================================================================================*/
	public abstract class WeaverTestBase {

		protected TestSchema Schema { get; private set; }
		protected WeaverInstance WeavInst { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[SetUp]
		protected virtual void SetUp() {
			Schema = new TestSchema();
			WeavInst = new WeaverInstance(Schema.Nodes, Schema.Rels);
		}

		/*--------------------------------------------------------------------------------------------* /
		[TearDown]
		public virtual void TearDown() {}*/

	}

}