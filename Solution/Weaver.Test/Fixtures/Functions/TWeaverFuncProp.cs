using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncProp {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Gremlin() {
			var f = new WeaverFuncProp<Person>(new Person(), (n => n.ExpectOneNode));

			Assert.AreEqual("ExpectOneNode", f.PropertyName, "Incorrect PropertyName.");
			Assert.AreEqual("ExpectOneNode", f.GremlinCode, "Incorrect GremlinCode.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GremlinBadExpression() {
			try {
				var f = new WeaverFuncProp<Person>(new Person(), (n => (n.ExpectOneNode == false)));
				Assert.Fail("Expected an Exception: "+f);
			}
			catch ( WeaverGremlinException e ) {
				Assert.NotNull(e);
			}
		}

	}

}