using Moq;
using NUnit.Framework;
using Weaver.Core.Graph;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Test.Core.Common.Vertices;

namespace Weaver.Test.Core.Fixtures.Graph {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverAllVertices : WeaverTestBase {

		private WeaverAllVertices vAllVerts;
		private Mock<IWeaverPath> vMockPath;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();

			var mockQuery = new Mock<IWeaverQuery>();
			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);

			vAllVerts = new WeaverAllVertices();
			vAllVerts.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Id() {
			Person p = vAllVerts.Id<Person>("5");

			Assert.NotNull(p, "Result should be filled.");
			vMockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepCustom>()), Times.Once());
			Assert.True(vAllVerts.ForSpecificId, "Incorrect ForSpecificId.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ExactIndex() {
			Person p = vAllVerts.ExactIndex<Person>(x => x.PersonId, 5);
			
			Assert.NotNull(p, "Result should be filled.");
			vMockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepExactIndex<Person>>()), Times.Once());
			Assert.False(vAllVerts.ForSpecificId, "Incorrect ForSpecificId.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true, "v")]
		[TestCase(false, "V")]
		public void BuildParameterizedString(bool pForId, string pExpect) {
			if ( pForId ) {
				vAllVerts.Id<Person>("x");
			}

			Assert.AreEqual(pExpect, vAllVerts.BuildParameterizedString(), "Incorrect result.");
		}

	}

}