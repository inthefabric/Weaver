using System.Collections.Generic;
using NUnit.Framework;
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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void BuildParameterizedString() {
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