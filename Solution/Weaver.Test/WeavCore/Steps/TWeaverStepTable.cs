using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Core.Pipe;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Test.Common.Schema;
using Weaver.Test.Common.Vertices;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Steps {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStepTable : WeaverTestBase {

		private Mock<IWeaverPath> vMockPath;
		private Mock<IWeaverQuery> vMockQuery;
		private WeaverStepTable vTable;
		private IWeaverVarAlias vAlias;
		private int vPathLen;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();
			vPathLen = 99;

			vMockQuery = new Mock<IWeaverQuery>();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Length).Returns(vPathLen);
			vMockPath.SetupGet(x => x.Query).Returns(vMockQuery.Object);

			var mockElem = new Mock<IWeaverElement>();
			mockElem.SetupGet(x => x.Path).Returns(vMockPath.Object);

			vTable = new WeaverStepTable(mockElem.Object, out vAlias);
			vTable.Path = vMockPath.Object;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			Assert.AreEqual(vAlias, vTable.Alias, "Incorrect Alias.");
			Assert.AreEqual("_TABLE"+vPathLen, vTable.Alias.Name, "Incorrect Alias.Name.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString() {
			string expect = "table(_TABLE"+vPathLen+"=new Table())";

			vMockPath.Setup(x => x.ItemAtIndex(0)).Returns((IWeaverPathItem)null);

			var asStep = new Mock<IWeaverStepAs>();
			vMockPath.Setup(x => x.ItemAtIndex(1)).Returns(asStep.Object);
			expect += "{}";

			vMockPath.Setup(x => x.ItemAtIndex(2)).Returns((IWeaverPathItem)null);

			var repStep = new Mock<IWeaverStepAsColumn>();
			repStep.SetupGet(x => x.ReplaceScript).Returns("replace.it()");
			vMockPath.Setup(x => x.ItemAtIndex(3)).Returns(repStep.Object);
			expect += "{replace.it()}";

			vMockPath.Setup(x => x.ItemAtIndex(4)).Returns((IWeaverPathItem)null);

			var propStep = new Mock<IWeaverStepAsColumn>();
			propStep.SetupGet(x => x.PropName).Returns("MyProp");
			vMockPath.Setup(x => x.ItemAtIndex(5)).Returns(propStep.Object);
			vMockQuery.Setup(x => x.AddStringParam("MyProp")).Returns("_P0");
			expect += "{it.getProperty(_P0)}";

			vMockPath.Setup(x => x.ItemAtIndex(6)).Returns((IWeaverPathItem)null);

			var propAppStep = new Mock<IWeaverStepAsColumn>();
			propAppStep.SetupGet(x => x.PropName).Returns("MyProp2");
			propAppStep.SetupGet(x => x.AppendScript).Returns(".get.data()");
			vMockPath.Setup(x => x.ItemAtIndex(7)).Returns(propAppStep.Object);
			vMockQuery.Setup(x => x.AddStringParam("MyProp2")).Returns("_P1");
			expect += "{it.getProperty(_P1).get.data()}";

			vMockPath.Setup(x => x.ItemAtIndex(8)).Returns((IWeaverPathItem)null);

			var objStep = new Mock<IWeaverStepAsColumn>();
			vMockPath.Setup(x => x.ItemAtIndex(9)).Returns(objStep.Object);
			expect += "{it}";

			vMockPath.Setup(x => x.ItemAtIndex(10)).Returns((IWeaverPathItem)null);

			var objAppStep = new Mock<IWeaverStepAsColumn>();
			objAppStep.SetupGet(x => x.AppendScript).Returns(".then.more()");
			vMockPath.Setup(x => x.ItemAtIndex(11)).Returns(objAppStep.Object);
			expect += "{it.then.more()}";

			vMockPath.Setup(x => x.IndexOfItem(vTable)).Returns(99);

			Console.WriteLine(expect);
			Assert.AreEqual(expect, vTable.BuildParameterizedString(), "Incorrect script.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedStringOrder() {
			vMockPath.Setup(x => x.ItemAtIndex(0)).Returns((IWeaverPathItem)null);

			var repStep = new Mock<IWeaverStepAsColumn>();
			repStep.SetupGet(x => x.ReplaceScript).Returns("A");
			vMockPath.Setup(x => x.ItemAtIndex(1)).Returns(repStep.Object);

			repStep = new Mock<IWeaverStepAsColumn>();
			repStep.SetupGet(x => x.ReplaceScript).Returns("B");
			vMockPath.Setup(x => x.ItemAtIndex(2)).Returns(repStep.Object);

			vMockPath.Setup(x => x.ItemAtIndex(3)).Returns((IWeaverPathItem)null);

			repStep = new Mock<IWeaverStepAsColumn>();
			repStep.SetupGet(x => x.ReplaceScript).Returns("C");
			vMockPath.Setup(x => x.ItemAtIndex(4)).Returns(repStep.Object);

			repStep = new Mock<IWeaverStepAsColumn>();
			repStep.SetupGet(x => x.ReplaceScript).Returns("D");
			vMockPath.Setup(x => x.ItemAtIndex(5)).Returns(repStep.Object);

			repStep = new Mock<IWeaverStepAsColumn>();
			repStep.SetupGet(x => x.ReplaceScript).Returns("E");
			vMockPath.Setup(x => x.ItemAtIndex(6)).Returns(repStep.Object);

			repStep = new Mock<IWeaverStepAsColumn>();
			repStep.SetupGet(x => x.ReplaceScript).Returns("F");
			vMockPath.Setup(x => x.ItemAtIndex(7)).Returns(repStep.Object);

			vMockPath.Setup(x => x.IndexOfItem(vTable)).Returns(99);

			string expect = "table(_TABLE"+vPathLen+"=new Table()){B}{A}{F}{E}{D}{C}";
			Console.WriteLine(expect);
			Assert.AreEqual(expect, vTable.BuildParameterizedString(), "Incorrect script.");
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void ColumnOrder() {
			IWeaverStepAs<Person> nullAlias;
			IWeaverStepAsColumn<Candy> candyPropAppendCol;
			IWeaverStepAsColumn<Candy> candyReplaceCol;
			IWeaverStepAsColumn<Candy> candyAppendCol;
			IWeaverVarAlias tableAlias;

			var path = WeavInst.Graph
				.V.ExactIndex<Person>(x => x.PersonId, 123)
					.AsColumn("PersObj")
					.As(out nullAlias)
				.OutLikesCandy.InVertex
					.AsColumn("CandyId", x => x.CandyId)
					.AsColumn("CandyCal", x => x.Calories, out candyPropAppendCol)
					.AsColumn("CandyReplace", out candyReplaceCol)
					.AsColumn("CandyAppend", out candyAppendCol)
				.Table(out tableAlias)
					.Iterate();

			//script adjustments must occur before using ToQuery()
			candyPropAppendCol.AppendScript = ".append.script()";
			candyReplaceCol.ReplaceScript = "123";
			candyAppendCol.AppendScript = ".outE.inV";

			IWeaverQuery q = path.ToQuery();

			const string expect = 
				"g.V('PerId',_P0)"+
					".as('PersObj')"+
					".as('step4')"+
				".outE('PLC').inV"+
					".as('CandyId')"+
					".as('CandyCal')"+
					".as('CandyReplace')"+
					".as('CandyAppend')"+
				".table(_TABLE11=new Table())"+
						"{}"+
						"{it}"+
						"{it.outE.inV}"+
						"{123}"+
						"{it.getProperty(_P1).append.script()}"+
						"{it.getProperty(_P2)}"+
					".iterate();";
			
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(123));
			expectParams.Add("_P1", new WeaverQueryVal(TestSchema.Candy_Calories));
			expectParams.Add("_P2", new WeaverQueryVal(TestSchema.Candy_CandyId));

			Assert.AreEqual(expect, q.Script, "Incorrect query script.");
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void BackAlias() {
			IWeaverStepAsColumn<Person> alias;

			IWeaverQuery q = WeavInst.Graph
				.V.ExactIndex<Person>(x => x.PersonId, 123)
					.AsColumn("PersObj", out alias)
				.OutLikesCandy.InVertex
					.Back(alias)
				.ToQuery();

			const string expect = 
				"g.V('PerId',_P0)"+
					".as('PersObj')"+
				".outE('PLC').inV"+
					".back('PersObj');";

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(123));

			Assert.AreEqual(expect, q.Script, "Incorrect query script.");
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
		}

	}

}