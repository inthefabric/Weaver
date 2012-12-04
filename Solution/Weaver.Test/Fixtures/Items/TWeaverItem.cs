using NUnit.Framework;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Items;
using Weaver.Test.Common;
using Weaver.Test.Common.Nodes;

namespace Weaver.Test.Fixtures.Items {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PathAndPathIndex() {
			var p = new TestPath();
			var n = p.BaseNode.OutHasPerson.ToNode;

			Assert.NotNull(n.Path, "Path should be filled.");
			Assert.AreEqual(2, n.PathIndex, "Incorrect PathIndex.");
			Assert.AreEqual(2, n.PathIndex, "Incorrect cached PathIndex.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PrevPathItem() {
			var p = new TestPath();
			var n = p.BaseNode.OutHasPerson.ToNode;
			var afterN = n.OutLikesCandy;
			Assert.AreEqual(n, afterN.PrevPathItem, "Incorrect PrevPathItem.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NextPathItem() {
			var p = new TestPath();
			var n = p.BaseNode.OutHasPerson.ToNode;
			var afterN = n.OutLikesCandy;
			Assert.AreEqual(afterN, n.NextPathItem, "Incorrect NextPathItem.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void QueryPathToThisItem() {
			var p = new TestPath();
			var i0 = p.BaseNode;
			var i1 = i0.OutHasPerson;
			var i2 = i1.ToNode;
			var i3 = i2.OutLikesCandy;
			var i4 = i3.ToNode;

			var items = i2.PathToThisItem;

			Assert.AreEqual(3, items.Count, "Incorrect PathToThisItem.Count.");
			Assert.AreEqual(i0, items[0], "Incorrect PathToThisItem[0].");
			Assert.AreEqual(i1, items[1], "Incorrect PathToThisItem[1].");
			Assert.AreEqual(i2, items[2], "Incorrect PathToThisItem[2].");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void QueryPathFromThisItem() {
			var p = new TestPath();
			var i0 = p.BaseNode;
			var i1 = i0.OutHasPerson;
			var i2 = i1.ToNode;
			var i3 = i2.OutLikesCandy;
			var i4 = i3.ToNode;

			var items = i2.PathFromThisItem;

			Assert.AreEqual(3, items.Count, "Incorrect PathFromThisItem.Count.");
			Assert.AreEqual(i2, items[0], "Incorrect PathFromThisItem[0].");
			Assert.AreEqual(i3, items[1], "Incorrect PathFromThisItem[1].");
			Assert.AreEqual(i4, items[2], "Incorrect PathFromThisItem[2].");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ExtensionAs() {
			Person alias;

			var p = new TestPath();
			Person personNodeA = p.BaseNode.OutHasPerson.ToNode;
			Person personNodeB = personNodeA.As(out alias);

			Assert.AreEqual(4, p.Length, "Incorrect Path.Length.");
			Assert.AreEqual(personNodeA, personNodeB, "Incorrectly returned Person Node.");

			var asFunc = (WeaverFuncAs<Person>)p.ItemAtIndex(3);
			Assert.AreEqual(p, asFunc.Path, "Incorrect AsFunc.Path.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ExtensionBack() {
			Person alias;

			var p = new TestPath();
			Person personNodeA = p.BaseNode.OutHasPerson.ToNode;
			Person personNodeB = personNodeA.As(out alias);
			Candy candyNode = personNodeB.OutLikesCandy.ToNode;
			Person personNodeC = candyNode.Back(alias);

			Assert.AreEqual(7, p.Length, "Incorrect Path.Length.");
			Assert.AreEqual(personNodeA, personNodeB, "Incorrectly returned Person Node (AB).");
			Assert.AreEqual(personNodeB, personNodeC, "Incorrectly returned Person Node (BC).");

			var backFunc = (WeaverFuncBack<Person>)p.ItemAtIndex(6);
			Assert.AreEqual(p, backFunc.Path, "Incorrect BackFunc.Path.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ExtensionProp() {
			var p = new TestPath();
			IWeaverProp prop = p.BaseNode.OutHasPerson.ToNode.Prop(x => x.Name);

			Assert.AreEqual(4, p.Length, "Incorrect Path.Length.");
			Assert.AreEqual(prop, p.ItemAtIndex(3), "Incorrect item at path index 3.");
			Assert.AreEqual(p, prop.Path, "Incorrect PropFunc.Path.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ExtensionHas() {
			var p = new TestPath();
			Person personNodeA = p.BaseNode.OutHasPerson.ToNode;
			Person personNodeB = personNodeA.Has(x => x.Name, WeaverFuncHasOp.EqualTo, "test");

			Assert.AreEqual(4, p.Length, "Incorrect Path.Length.");
			Assert.AreEqual(personNodeA, personNodeB, "Incorrectly returned Person Node.");

			var hasFunc = (WeaverFuncHas<Person>)p.ItemAtIndex(3);
			Assert.AreEqual(p, hasFunc.Path, "Incorrect BackFunc.Path.");
		}

	}

}