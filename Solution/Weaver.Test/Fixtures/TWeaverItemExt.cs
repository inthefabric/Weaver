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

			IWeaverFuncAs<TestItem> alias;
			TestItem result = item.As(out alias);

			Assert.AreEqual(item, result, "Incorrect result.");
			Assert.NotNull(alias, "Alias should be filled.");
			Assert.AreEqual(item, alias.Item, "Incorrect Alias.Item.");
			item.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverFuncAs<TestItem>>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Back() {
			var item = new TestItem();
			
			var mockTestItem = new Mock<TestItem>();
			mockTestItem.SetupGet(x => x.PathIndex).Returns(4);
			
			var mockAlias = new Mock<IWeaverFuncAs<TestItem>>();
			mockAlias.SetupGet(x => x.Item).Returns(mockTestItem.Object);

			TestItem result = item.Back(mockAlias.Object);

			Assert.AreEqual(mockTestItem.Object, result, "Incorrect result.");
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
		/*--------------------------------------------------------------------------------------------* /
		[Test]
		public void Aggregate() {
			var tx = new WeaverTransaction();
			IWeaverListVar x;
			
			tx.AddQuery(
				WeaverTasks.InitListVar(tx, out x)
			);
			
			tx.AddQuery(
				WeaverTasks.BeginPath<Root>(new Root()).BaseNode
				.OutHasPerson.ToNode
					.Aggregate(x).Iterate()
				.End()
			);
			
			tx.Finish(WeaverTransaction.ConclusionType.Success, x);
			Console.WriteLine(tx.Script.Replace(";", ";\n"));
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void End() {
			const string parScript = "this.is.the.parameterized.script";

			var mockQuery = new Mock<IWeaverQuery>();

			var item = new TestItem();
			item.MockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);
			item.MockPath.Setup(x => x.BuildParameterizedScript()).Returns(parScript);

			IWeaverQuery result = item.End();

			Assert.AreEqual(mockQuery.Object, result, "Incorrect result.");
			mockQuery.Verify(x => x.FinalizeQuery(parScript), Times.Once());
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void EndAsVar() {
			const string name = "_var0";
			const string parScript = "this.is.the.parameterized.script";
			const string parScriptWithVar = name+"="+parScript;

			var mockQuery = new Mock<IWeaverQuery>();

			var item = new TestItem();
			item.MockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);
			item.MockPath.Setup(x => x.BuildParameterizedScript()).Returns(parScript);

			var mockTx = new Mock<IWeaverTransaction>();
			mockTx.Setup(x => x.GetNextVarName()).Returns(name);

			IWeaverQuery result = item.EndAsVar(new WeaverVarAlias(mockTx.Object));

			Assert.AreEqual(mockQuery.Object, result, "Incorrect result.");
			mockQuery.Verify(x => x.FinalizeQuery(parScriptWithVar), Times.Once());
		}

	}

}