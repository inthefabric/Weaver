namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepCustom : WeaverStep {

		private readonly string vScript;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepCustom(string pScript, bool pSkipDotPrefix=false) {
			vScript = pScript;
			SkipDotPrefix = pSkipDotPrefix;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return vScript;
		}

	}

}