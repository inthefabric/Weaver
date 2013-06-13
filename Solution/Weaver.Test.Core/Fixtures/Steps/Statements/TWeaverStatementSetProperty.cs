using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps.Statements;
using Weaver.Test.Core.Common.Schema;
using Weaver.Test.Core.Common.Vertices;
using Weaver.Test.Core.Utils;

namespace Weaver.Test.Core.Fixtures.Steps.Statements {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStatementSetProperty : WeaverTestBase {

		private Mock<IWeaverPath> vMockPath;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(new WeaverQuery());
			vMockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyName() {
			var setProp = new WeaverStatementSetProperty<Person>(vMockPath.Object, x => x.PersonId, 5);

			Assert.AreEqual(TestSchema.Person_PersonId, setProp.PropertyName,
				"Incorrect PropertyName.");
			Assert.AreEqual(TestSchema.Person_PersonId, setProp.PropertyName,
				"Incorrect cached PropertyName.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyNameInvalid() {
			var setProp = new WeaverStatementSetProperty<Person>(
				vMockPath.Object, x => (x.PersonId == 99), 5);

			WeaverTestUtil.CheckThrows<WeaverStatementException<Person>>(true, () => {
				var p = setProp.PropertyName;
			});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			var setProp = new WeaverStatementSetProperty<Person>(vMockPath.Object, x => x.PersonId, 5);

			const string expect = "it.setProperty('"+TestSchema.Person_PersonId+"',_P0)";
			Assert.AreEqual(expect, setProp.BuildParameterizedString(), "Incorrect result.");
		}

	}

}