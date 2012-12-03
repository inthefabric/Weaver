namespace Weaver.Test.Fixtures.Items {

	/*================================================================================================*/
	//[TestFixture]
	public class TWeaverItem {


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