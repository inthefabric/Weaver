using NUnit.Framework;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Test.Core.Common.Vertices;

namespace Weaver.Test.Core.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverInstance : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Basic() {
			IWeaverQuery q = WeavInst.Graph
				.V.ExactIndex<Person>(x => x.Name, "Zach")
				.InPersonKnows.FromNode
				.Has(x => x.IsMale, WeaverStepHasOp.EqualTo, true)
				.ToQuery();

			const string expect = "g.V('Name',_P0).inE('PKP').outV.has('IsMale',Tokens.T.eq,_P1);";
			Assert.AreEqual(expect, q.Script, "Incorrect script.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Next() {
			IWeaverQuery q = WeavInst.Graph
				.V.ExactIndex<Person>(x => x.Name, "Zach")
				.Next()
				.ToQuery();

			const string expect = "g.V('Name',_P0).next();";
			Assert.AreEqual(expect, q.Script, "Incorrect script.");
		}

	}

}