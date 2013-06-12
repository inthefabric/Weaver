using Moq;
using NUnit.Framework;
using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Test.Core.Common.Edges;
using Weaver.Test.Core.Common.Vertices;

namespace Weaver.Test.Core.Fixtures.Elements {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverVertex : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(false, false, false, "inV")]
		[TestCase(false, false, true, "inV")]
		[TestCase(false, true, false, "outV")]
		[TestCase(false, true, true, "outV")]
		[TestCase(true, false, false, "v(0)")]
		[TestCase(true, false, true, "v(0)")]
		[TestCase(true, true, false, "v(0)")]
		[TestCase(true, true, true, "v(0)")]
		public void BuildParameterizedString(bool pIsRoot, bool pIsFrom, bool pExpectOne,
																				string pExpectScript) {
			IWeaverVertex n;
			
			if ( pIsRoot ) {
				n = new Root { IsFromVertex = pIsFrom, ExpectOneVertex = pExpectOne };
			}
			else {
				n = new Person { IsFromVertex = pIsFrom, ExpectOneVertex = pExpectOne };
			}

			Assert.AreEqual(pExpectScript, n.BuildParameterizedString(), "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewRel() {
			var mockPath = new Mock<IWeaverPath>();
			var person = new Person { Path = mockPath.Object };

			PersonLikesCandy rel = person.OutLikesCandy;

			Assert.NotNull(rel, "Rel should be filled.");
			mockPath.Verify(x => x.AddItem(It.IsAny<PersonLikesCandy>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ItemIdentifier() {
			const string id = "1234:3465:99";
			var p = new Person() { Id = id };
			Assert.AreEqual("Person(Id="+id+")", p.ItemIdentifier, "Incorrect Vertex.ItemIdentifier.");
		}

	}

}