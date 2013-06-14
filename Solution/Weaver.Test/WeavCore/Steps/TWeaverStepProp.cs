using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Steps;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Steps {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStepProp : WeaverTestBase {

		private Mock<IWeaverPath> vMockPath;
		private WeaverStepProp<Person> vFuncProp;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);

			vFuncProp = new WeaverStepProp<Person>(n => n.PersonId);
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
			vFuncProp = new WeaverStepProp<Person>(n => (n.PersonId == 99));
			vFuncProp.Path = vMockPath.Object;

			WeaverTestUtil.CheckThrows<WeaverStepException>(true, () => {
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
			vFuncProp = new WeaverStepProp<Person>(n => n.Id);
			vFuncProp.Path = vMockPath.Object;
			Assert.AreEqual("id", vFuncProp.BuildParameterizedString(), "Incorrect result.");
		}

	}

}