using NUnit.Framework;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverUpdate : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildIntString() {
			var u = new WeaverUpdate<Person>(new Person { PersonId = 123 }, x => x.PersonId);
			Assert.AreEqual("PersonId", u.PropName, "Incorrect PropName.");
			Assert.NotNull(u.PropValue, "PropVal should not be null.");
			Assert.AreEqual(123, u.PropValue.Original, "Incorrect PropVal.Original.");
			Assert.False(u.PropValue.AllowQuote, "Incorrect PropVal.AllowQuote.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildTextString() {
			var u = new WeaverUpdate<Person>(new Person { Name = "test" }, x => x.Name);
			Assert.AreEqual("Name", u.PropName, "Incorrect PropName.");
			Assert.NotNull(u.PropValue, "PropVal should not be null.");
			Assert.AreEqual("test", u.PropValue.Original, "Incorrect PropVal.Original.");
			Assert.False(u.PropValue.AllowQuote, "Incorrect PropVal.AllowQuote.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildEmptyTextString() {
			var u = new WeaverUpdate<Person>(new Person { Name = "" }, x => x.Name);
			Assert.AreEqual("Name", u.PropName, "Incorrect PropName.");
			Assert.NotNull(u.PropValue, "PropVal should not be null.");
			Assert.AreEqual("", u.PropValue.Original, "Incorrect PropVal.Original.");
			Assert.False(u.PropValue.AllowQuote, "Incorrect PropVal.AllowQuote.");
		}

	}

}