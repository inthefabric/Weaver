using Moq;
using NUnit.Framework;
using Weaver.Core.Graph;
using Weaver.Core.Path;
using Weaver.Core.Steps;
using Weaver.Test.Core.Common.Edges;

namespace Weaver.Test.Core.Fixtures.Graph {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverAllEdges : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var mockPath = new Mock<IWeaverPath>();
			var ae = new WeaverAllEdges();
			ae.Path = mockPath.Object;

			PersonLikesCandy p = ae.ExactIndex<PersonLikesCandy>(x => x.TimesEaten, 5);
			
			Assert.NotNull(p, "Result should be filled.");

			mockPath.Verify(
				x => x.AddItem(It.IsAny<WeaverStepExactIndex<PersonLikesCandy>>()), Times.Once()
			);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			var ae = new WeaverAllEdges();
			Assert.AreEqual("E", ae.BuildParameterizedString(), "Incorrect result.");
		}

	}

}