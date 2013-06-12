using Moq;
using NUnit.Framework;
using Weaver.Core.Graph;
using Weaver.Core.Path;
using Weaver.Core.Steps;
using Weaver.Test.Core.Common.Vertices;

namespace Weaver.Test.Core.Fixtures.Graph {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverAllVertices : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var mockPath = new Mock<IWeaverPath>();
			var av = new WeaverAllVertices();
			av.Path = mockPath.Object;

			Person p = av.ExactIndex<Person>(x => x.PersonId, 5);
			
			Assert.NotNull(p, "Result should be filled.");
			mockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepExactIndex<Person>>()), Times.Once());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			var av = new WeaverAllVertices();
			Assert.AreEqual("V", av.BuildParameterizedString(), "Incorrect result.");
		}

	}

}