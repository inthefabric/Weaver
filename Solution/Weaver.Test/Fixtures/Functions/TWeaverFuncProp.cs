using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncProp : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyName() {
			var f = new WeaverFuncProp<Person>(n => n.PersonId);
			Assert.AreEqual("PersonId", f.PropertyName, "Incorrect PropertyName.");
			Assert.AreEqual("PersonId", f.PropertyName, "Incorrect cached PropertyName.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyNameInvalid() {
			var f = new WeaverFuncProp<Person>(n => (n.ExpectOneNode == false));

			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, () => {
				var p = f.PropertyName;
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			var f = new WeaverFuncProp<Person>(n => n.PersonId);
			Assert.AreEqual("PersonId", f.BuildParameterizedString(), "Incorrect result.");
		}

	}

}