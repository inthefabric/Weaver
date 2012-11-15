namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	//[TestFixture]
	public class TWeaverQuery {

		//TODO: Weaver.PathLength
		//TODO: Weaver.FindAsNode
		//TODO: Weaver.FindAsNode bounds
		//TODO: Weaver.PathAtIndex bounds
		//TODO: Weaver.PathToIndex bounds
		//TODO: Weaver.PathFromIndex bounds


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void QueryA() {
			var q = new FabricQuery();
			q.Root
				.OutHasThing.ToThing
				.As<IQueryableThing>("test")
				.OutHasArtifact.ToArtifact
					.Has<Artifact>(a => a.ArtifactId, WeaverFuncHasOp.LessThanOrEqualTo, 5)
				.InMemberCreates.FromMember
				.Back<IQueryableThing>("test")
				.OutHasArtifact.ToArtifact
				.InMemberCreates.FromMember
				.OutUsesApp.ToApp.Prop<App>(a => a.AppId);

			Assert.AreEqual("g.v(0)"+
					".outE('RootHasThing').inV"+
					".as('test')"+
					".outE('ThingHasArtifact').inV(0)"+
						".has('ArtifactId', T.lte, 5)"+
					".inE('MemberCreatesArtifact').outV(0)"+
					".back('test')"+
					".outE('ThingHasArtifact').inV(0)"+
					".inE('MemberCreatesArtifact').outV(0)"+
					".outE('MemberUsesApp').inV(0).AppId", 
				q.GremlinCode, "Incorrect GrelminCode.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void PrevQueryItem() {
			var q = new FabricQuery();
			var n = q.Root.OutHasThing.ToThing;
			var afterN = n.OutHasArtifact;
			Assert.AreEqual(n, afterN.PrevQueryItem, "Incorrect PrevQueryItem.");
		}

		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void NextQueryItem() {
			var q = new FabricQuery();
			var n = q.Root.OutHasThing.ToThing;
			var afterN = n.OutHasArtifact;
			Assert.AreEqual(afterN, n.NextQueryItem, "Incorrect NextQueryItem.");
		}

		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void QueryPathToThisItem() {
			var q = new FabricQuery();
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
			var q = new FabricQuery();
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