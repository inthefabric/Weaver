using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Test.Common;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Items {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverItem : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Path() {
			var item = new TestItem();
			Assert.AreEqual(item.MockPath.Object, item.Path, "Incorrect Path.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PathIndex() {
			const int expectIndex = 2;

			var item = new TestItem();
			item.MockPath.Setup(x => x.IndexOfItem(item)).Returns(expectIndex);

			Assert.AreEqual(expectIndex, item.PathIndex, "Incorrect PathIndex.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PathIndexFail() {
			var candy = new Candy();

			WeaverTestUtils.CheckThrows<WeaverException>(true, () => {
				var i = candy.PathIndex;
			});
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PrevPathItem() {
			IWeaverItem prev = new Mock<IWeaverItem>().Object;

			var item = new TestItem();
			item.MockPath.Setup(x => x.IndexOfItem(item)).Returns(4);
			item.MockPath.Setup(x => x.ItemAtIndex(3)).Returns(prev);

			Assert.AreEqual(prev, item.PrevPathItem, "Incorrect PrevPathItem.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NextPathItem() {
			IWeaverItem next = new Mock<IWeaverItem>().Object;

			var item = new TestItem();
			item.MockPath.Setup(x => x.IndexOfItem(item)).Returns(4);
			item.MockPath.Setup(x => x.ItemAtIndex(5)).Returns(next);

			Assert.AreEqual(next, item.NextPathItem, "Incorrect NextPathItem.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PathToThisItem() {
			IWeaverItem itemA = new Mock<IWeaverItem>().Object;
			IWeaverItem itemB = new Mock<IWeaverItem>().Object;
			IWeaverItem itemC = new Mock<IWeaverItem>().Object;
			var expectItems = new List<IWeaverItem> { itemA, itemB, itemC };

			var item = new TestItem();
			item.MockPath.Setup(x => x.IndexOfItem(item)).Returns(4);
			item.MockPath.Setup(x => x.PathToIndex(4, true)).Returns(expectItems);

			Assert.AreEqual(expectItems, item.PathToThisItem, "Incorrect PathToThisItem.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PathFromThisItem() {
			IWeaverItem itemA = new Mock<IWeaverItem>().Object;
			IWeaverItem itemB = new Mock<IWeaverItem>().Object;
			IWeaverItem itemC = new Mock<IWeaverItem>().Object;
			var expectItems = new List<IWeaverItem> { itemA, itemB, itemC };

			var item = new TestItem();
			item.MockPath.Setup(x => x.IndexOfItem(item)).Returns(4);
			item.MockPath.Setup(x => x.PathFromIndex(4, true)).Returns(expectItems);

			Assert.AreEqual(expectItems, item.PathFromThisItem, "Incorrect PathFromThisItem.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		//TEST: uncomment TWeaverItemExt tests
		/*--------------------------------------------------------------------------------------------* /
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

		/*--------------------------------------------------------------------------------------------* /
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

		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void ExtensionProp() {
			var p = new TestPath();
			IWeaverProp prop = p.BaseNode.OutHasPerson.ToNode.Prop(x => x.Name);

			Assert.AreEqual(4, p.Length, "Incorrect Path.Length.");
			Assert.AreEqual(prop, p.ItemAtIndex(3), "Incorrect item at path index 3.");
			Assert.AreEqual(p, prop.Path, "Incorrect PropFunc.Path.");
		}

		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void ExtensionHas() {
			var p = new TestPath();
			Person personNodeA = p.BaseNode.OutHasPerson.ToNode;
			Person personNodeB = personNodeA.Has(x => x.Name, WeaverFuncHasOp.EqualTo, "test");

			Assert.AreEqual(4, p.Length, "Incorrect Path.Length.");
			Assert.AreEqual(personNodeA, personNodeB, "Incorrectly returned Person Node.");

			var hasFunc = (WeaverFuncHas<Person>)p.ItemAtIndex(3);
			Assert.AreEqual(p, hasFunc.Path, "Incorrect BackFunc.Path.");
		}*/

	}

}