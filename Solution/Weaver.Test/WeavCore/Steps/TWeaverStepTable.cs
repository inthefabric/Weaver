using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Core.Pipe;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Core.Steps.Helpers;
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
			var alias = new WeaverVarAlias("t");
			const string customScript = ".what.does.this.do()";

			var cols = new WeaverTableColumns(WeavInst.Config);
			cols.AddObjectColumn<Person>();
			cols.AddNullColumn<Person>();
			cols.AddPropertyColumn<Candy>(x => x.CandyId);
			cols.AddPropertyColumn<Candy>(x => x.Calories, customScript);

			IWeaverStepAs<Person> c0;
			IWeaverStepAs<Person> c1;
			IWeaverStepAs<Candy> c2;
			IWeaverStepAs<Candy> c3;

			IWeaverQuery q = WeavInst.Graph
				.V.ExactIndex<Person>(x => x.PersonId, 123)
					.As(out c0)
					.As(out c1)
				.OutLikesCandy.InVertex
					.As(out c2)
					.As(out c3)
				.Table(alias, cols)
				.ToQuery();

			const string expect = 
				"g.V('PerId',_P0)"+
					".as('step3')"+
					".as('step4')"+
				".outE('PLC').inV"+
					".as('step7')"+
					".as('step8')"+
				".table(t)"+
					"{it}"+
					"{}"+
					"{it.property(_P1)}"+
					"{it.property(_P2)"+customScript+"};";
			
			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_P0", new WeaverQueryVal(123));
			expectParams.Add("_P1", new WeaverQueryVal(TestSchema.Candy_CandyId));
			expectParams.Add("_P2", new WeaverQueryVal(TestSchema.Candy_Calories));

			var expectAliases = new[] { "step3", "step4", "step7", "step8" };

			Assert.AreEqual(expect, q.Script, "Incorrect query script.");
			WeaverTestUtil.CheckQueryParamsOriginalVal(q, expectParams);
			Assert.AreEqual(expectAliases, cols.GetColumnAliases(), "Incorrect aliases.");
		}

	}

}