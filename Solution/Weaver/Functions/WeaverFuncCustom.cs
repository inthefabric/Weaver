using Weaver.Items;

namespace Weaver.Functions {

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