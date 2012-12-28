using NUnit.Framework;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverUpdate : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildStrings() {
			var u = new WeaverUpdate<Person>(new Person { PersonId = 123 }, x => x.PersonId);
			Assert.AreEqual("PersonId", u.PropName, "Incorrect PropName.");
			Assert.NotNull(u.PropValue, "PropVal should not be null.");
			Assert.AreEqual(123, u.PropValue.Original, "Incorrect PropVal.Original.");
		}

	}

}