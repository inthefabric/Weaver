using Moq;
using NUnit.Framework;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;

namespace Weaver.Test.Fixtures.Items {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverNode : WeaverTestBase {


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
			IWeaverNode n;
			
			if ( pIsRoot ) {
				n = new Root { IsFromNode = pIsFrom, ExpectOneNode = pExpectOne };
			}
			else {
				n = new Person { IsFromNode = pIsFrom, ExpectOneNode = pExpectOne };
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
			Assert.AreEqual("Person(Id="+id+")", p.ItemIdentifier, "Incorrect Node.ItemIdentifier.");
		}

	}

}