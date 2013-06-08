namespace Weaver.Core.Func {

	/*================================================================================================*/
	public class WeaverFuncCustom : WeaverFunc {

		private readonly string vScript;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncCustom(string pScript, bool pSkipDotPrefix=false) {
			vScript = pScript;
			SkipDotPrefix = pSkipDotPrefix;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return vScript;
		}

	}

}