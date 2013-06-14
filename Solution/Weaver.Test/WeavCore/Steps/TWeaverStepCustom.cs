using NUnit.Framework;
using Weaver.Core.Steps;

namespace Weaver.Test.WeavCore.Steps {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverStepCustom : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("testSkip", true)]
		[TestCase("testNoSkip", false)]
		public void BuildParameterizedString(string pScript, bool pSkipDot) {
			var sc = new WeaverStepCustom(pScript, pSkipDot);
			Assert.AreEqual(pScript, sc.BuildParameterizedString(), "Incorrect result.");
		}

	}

}