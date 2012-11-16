﻿using NUnit.Framework;
using Weaver.Functions;
using Weaver.Test.Common;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Rels;

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
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Gremlin() {
			var q = new TestQuery();
			q.Root
				.OutHasPerson.ToNode
				.InRootHas.FromNode
				.OutHasPerson.ToNode
					.As<IQueryPerson>("test")
				.OutKnowsPerson.ToNode
					.Has<Person>(p => p.PersonId, WeaverFuncHasOp.LessThanOrEqualTo, 5)
				.InPersonKnows.FromNode
					.Back<IQueryPerson>("test")
				.OutLikesCandy
					.Has<PersonLikesCandy>(h => h.Enjoyment, WeaverFuncHasOp.GreterThanOrEqualTo, 0.2)
					.ToNode
				.InPersonLikes.FromNode
					.Prop<Person>(p => p.Name);

			const string expect = "g.v(0)"+
				".outE('RootHasPerson').inV"+
				".inE('RootHasPerson')[0].outV(0)"+
				".outE('RootHasPerson').inV"+
					".as('test')"+
				".outE('PersonKnowsPerson').inV"+
					".has('PersonId', T.lte, 5)"+
				".inE('PersonKnowsPerson').outV"+
					".back('test')"+
				".outE('PersonLikesCandy')"+
					".has('Enjoyment', T.gte, 0.2)"+
					".inV"+
				".inE('PersonLikesCandy').outV"+
					".Name";

			Assert.AreEqual(expect, q.GremlinCode, "Incorrect GrelminCode.");
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