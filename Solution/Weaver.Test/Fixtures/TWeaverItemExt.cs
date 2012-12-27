using Moq;
using NUnit.Framework;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverItemExt : WeaverTestBase {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void As() {
			var item = new TestItem();
			item.MockPath.SetupGet(x => x.Length).Returns(4);

			TestItem alias;
			TestItem result = item.As(out alias);

			Assert.AreEqual(item, result, "Incorrect result.");
			Assert.AreEqual(item, alias, "Incorrect alias.");
			item.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverFuncAs<TestItem>>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Back() {
			var alias = new TestItem();

			var item = new TestItem();
			item.MockPath.Setup(x => x.IndexOfItem(alias)).Returns(4);

			TestItem result = item.Back(alias);

			Assert.AreEqual(alias, result, "Incorrect result.");
			item.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverFuncBack<TestItem>>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Prop() {
			var item = new TestItem();
			IWeaverItemWithPath result = item.Prop(x => x.ItemIdentifier);

			Assert.NotNull(result, "Result should be filled.");
			item.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverFuncProp<TestItem>>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Has() {
			var item = new TestItem();
			TestItem result = item.Has(x => x.ItemIdentifier, WeaverFuncHasOp.EqualTo, 1);

			Assert.AreEqual(item, result, "Incorrect result.");
			item.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverFuncHas<TestItem>>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void UpdateEach() {
			var item = new TestItem();
			var updates = new WeaverUpdates<TestItem>();
			updates.AddUpdate(item, p => p.ItemIdentifier);

			TestItem result = item.UpdateEach(updates);

			Assert.AreEqual(item, result, "Incorrect result.");
			item.MockPath
				.Verify(x => x.AddItem(It.IsAny<WeaverFuncUpdateEach<TestItem>>()), Times.Once());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void End() {
			const string parScript = "this.is.the.parameterized.script;";

			var mockQuery = new Mock<IWeaverQuery>();
			//mockQuery.Setup(x => x.FinalizeQuery(parScript));

			var item = new TestItem();
			item.MockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);
			item.MockPath.Setup(x => x.BuildParameterizedScript()).Returns(parScript);

			IWeaverQuery result = item.End();

			Assert.AreEqual(mockQuery.Object, result, "Incorrect result.");
			mockQuery.Verify(x => x.FinalizeQuery(parScript), Times.Once());
		}

	}

}