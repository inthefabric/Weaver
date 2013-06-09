using Weaver.Core.Steps;

namespace Weaver.Core.Exceptions {

	/*================================================================================================*/
	public class WeaverStepException : WeaverException {

		public IWeaverStep Step { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepException(IWeaverStep pStep, string pMessage) :
														base("Step", pStep.ItemIdentifier, pMessage) {
			Step = pStep;
		}

	}

}