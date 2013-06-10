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
			IWeaverQuery q = WeavInst.Graph.V.ExactIndex<Person>(x => x.Name, "Zach")
				.InPersonKnows.FromNode
				.Has<Person>(x => x.IsMale, WeaverStepHasOp.EqualTo, true)
				.ToQuery();

			Assert.Fail(q.Script);
		}

	}

}