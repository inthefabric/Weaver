using NUnit.Framework;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures.Items {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(false, false, false, "inV")]
		[TestCase(false, false, true, "inV(0)")]
		[TestCase(false, true, false, "outV")]
		[TestCase(false, true, true, "outV(0)")]
		[TestCase(true, false, false, "v(0)")]
		[TestCase(true, false, true, "v(0)")]
		[TestCase(true, true, false, "v(0)")]
		[TestCase(true, true, true, "v(0)")]
		public void Gremlin(bool pIsRoot, bool pIsFrom, bool pExpectOne, string pExpectGremlin) {
			IWeaverNode n;
			if ( pIsRoot ) { n = new Root { IsFromNode = pIsFrom, ExpectOneNode = pExpectOne }; }
			else { n = new Person { IsFromNode = pIsFrom, ExpectOneNode = pExpectOne }; }

			Assert.AreEqual(pExpectGremlin, n.GremlinCode, "Incorrect GremlinCode.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void PrevQueryItem() {
			var q = new TestQuery();
			var n = q.Root.OutHasThing.ToThing;
			var afterN = n.OutHasArtifact;
			Assert.AreEqual(n, afterN.PrevQueryItem, "Incorrect PrevQueryItem.");
		}

		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void NextQueryItem() {
			var q = new TestQuery();
			var n = q.Root.OutHasThing.ToThing;
			var afterN = n.OutHasArtifact;
			Assert.AreEqual(afterN, n.NextQueryItem, "Incorrect NextQueryItem.");
		}

		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void QueryPathToThisItem() {
			var q = new TestQuery();
			var n = q.Root.OutHasThing.ToThing;
			int pathCountAtN = q.PathLength();
			var afterN = n.OutHasArtifact.ToArtifact;
			var listToN = n.QueryPathToThisItem;

			Assert.AreEqual(pathCountAtN, listToN.Count, "Incorrect QueryPathToThisItem.Count.");

			for ( int i = 0 ; i < pathCountAtN ; ++i ) {
				Assert.AreEqual(listToN[i], n.QueryPathToThisItem[i],
					"Incorrect QueryPathToThisItem["+i+"].");
			}
		}

		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void QueryPathFromThisItem() {
			var q = new TestQuery();
			var n = q.Root.OutHasThing.ToThing;
			int pathCountAtN = q.PathLength();
			var afterN = n.OutHasArtifact.ToArtifact;
			var listFromN = n.QueryPathFromThisItem;
			int pathCountAtAfterN = q.PathLength();
			int expectCount = pathCountAtAfterN-pathCountAtN+1;

			Assert.AreEqual(expectCount, listFromN.Count, "Incorrect QueryPathFromThisItem.Count.");

			for ( int i = 0 ; i < expectCount ; ++i ) {
				Assert.AreEqual(listFromN[i], n.QueryPathFromThisItem[i],
					"Incorrect QueryPathFromThisItem["+i+"].");
			}
		}*/

	}

}