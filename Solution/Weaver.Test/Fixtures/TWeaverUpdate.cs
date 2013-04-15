using NUnit.Framework;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Schema;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverUpdate : WeaverTestBase {

		private WeaverConfig vConfig;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();
			vConfig = new WeaverConfig(Schema.Nodes, Schema.Rels);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildIntString() {
			var u = new WeaverUpdate<Person>(vConfig, new Person { PersonId = 123 }, x => x.PersonId);
			Assert.AreEqual(TestSchema.Person_PersonId, u.PropName, "Incorrect PropName.");
			Assert.NotNull(u.PropValue, "PropVal should not be null.");
			Assert.AreEqual(123, u.PropValue.Original, "Incorrect PropVal.Original.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildTextString() {
			var u = new WeaverUpdate<Person>(vConfig, new Person { Name = "test" }, x => x.Name);
			Assert.AreEqual("Name", u.PropName, "Incorrect PropName.");
			Assert.NotNull(u.PropValue, "PropVal should not be null.");
			Assert.AreEqual("test", u.PropValue.Original, "Incorrect PropVal.Original.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildEmptyTextString() {
			var u = new WeaverUpdate<Person>(vConfig, new Person { Name = "" }, x => x.Name);
			Assert.AreEqual("Name", u.PropName, "Incorrect PropName.");
			Assert.NotNull(u.PropValue, "PropVal should not be null.");
			Assert.AreEqual("", u.PropValue.Original, "Incorrect PropVal.Original.");
		}

	}

}