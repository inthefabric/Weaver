using Moq;
using NUnit.Framework;
using Weaver.Core.Path;
using Weaver.Core.Steps;
using Weaver.Test.Core.Common.Vertices;

namespace Weaver.Test.Core.Fixtures.Steps {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStepAs : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			const int pathLen = 99;
			const int itemI = pathLen;

			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Length).Returns(pathLen);
			
			var mockPerson = new Mock<Person>();
			mockPerson.SetupGet(x => x.Path).Returns(mockPath.Object);

			var f = new WeaverStepAs<Person>(mockPerson.Object);

			Assert.AreEqual("step"+itemI, f.Label, "Incorrect Label.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			const int pathLen = 99;
			const int itemI = pathLen;

			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Length).Returns(pathLen);
			
			var mockPerson = new Mock<Person>();
			mockPerson.SetupGet(x => x.Path).Returns(mockPath.Object);

			var f = new WeaverStepAs<Person>(mockPerson.Object);

			Assert.AreEqual("as('step"+itemI+"')", f.BuildParameterizedString(), "Incorrect result.");
		}

	}

}