using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Schema;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncProp : WeaverTestBase {

		private Mock<IWeaverPath> vMockPath;
		private WeaverFuncProp<Person> vFuncProp;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);

			vFuncProp = new WeaverFuncProp<Person>(n => n.PersonId);
			vFuncProp.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyName() {
			Assert.AreEqual(TestSchema.Person_PersonId, vFuncProp.PropertyName,
				"Incorrect PropertyName.");
			Assert.AreEqual(TestSchema.Person_PersonId, vFuncProp.PropertyName,
				"Incorrect cached PropertyName.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PropertyNameInvalid() {
			vFuncProp = new WeaverFuncProp<Person>(n => (n.PersonId == 99));
			vFuncProp.Path = vMockPath.Object;

			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, () => {
				var p = vFuncProp.PropertyName;
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			Assert.AreEqual("property('"+TestSchema.Person_PersonId+"')",
				vFuncProp.BuildParameterizedString(), "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedStringId() {
			vFuncProp = new WeaverFuncProp<Person>(n => n.Id);
			vFuncProp.Path = vMockPath.Object;
			Assert.AreEqual("id", vFuncProp.BuildParameterizedString(), "Incorrect result.");
		}

	}

}