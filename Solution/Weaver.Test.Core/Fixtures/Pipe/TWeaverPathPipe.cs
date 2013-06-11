using Moq;
using NUnit.Framework;
using Weaver.Core.Steps;
using Weaver.Test.Core.Common;

namespace Weaver.Test.Core.Fixtures.Pipe {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverPathPipe : WeaverTestBase {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("count()")]
		public void Count(string pExpect) {
			var elem = new TestElement();
			VerifyCustom(elem, (WeaverStepCustom)elem.Count(), pExpect);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void VerifyCustom(TestElement pElem, WeaverStepCustom pCustom, string pExpect) {
			Assert.AreEqual(pExpect, pCustom.BuildParameterizedString(), "Incorrect result.");
			pElem.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepCustom>()), Times.Once());
		}

	}

}