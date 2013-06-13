using Moq;
using NUnit.Framework;
using Weaver.Core.Graph;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Test.Core.Common.Edges;

namespace Weaver.Test.Core.Fixtures.Graph {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverAllEdges : WeaverTestBase {

		private WeaverAllEdges vAllEdges;
		private Mock<IWeaverPath> vMockPath;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			var mockQuery = new Mock<IWeaverQuery>();
			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);

			vAllEdges = new WeaverAllEdges();
			vAllEdges.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Id() {
			PersonLikesCandy p = vAllEdges.Id<PersonLikesCandy>("abc-123");

			Assert.NotNull(p, "Result should be filled.");
			vMockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepCustom>()), Times.Once());
			Assert.True(vAllEdges.ForSpecificId, "Incorrect ForSpecificId.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ExactIndex() {
			PersonLikesCandy p = vAllEdges.ExactIndex<PersonLikesCandy>(x => x.TimesEaten, 5);

			Assert.NotNull(p, "Result should be filled.");
			vMockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepExactIndex<PersonLikesCandy>>()),
				Times.Once());
			Assert.False(vAllEdges.ForSpecificId, "Incorrect ForSpecificId.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true, "e")]
		[TestCase(false, "E")]
		public void BuildParameterizedString(bool pForId, string pExpect) {
			if ( pForId ) {
				vAllEdges.Id<PersonLikesCandy>("x");
			}

			Assert.AreEqual(pExpect, vAllEdges.BuildParameterizedString(), "Incorrect result.");
		}

	}

}