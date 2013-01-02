using Weaver.Interfaces;

namespace Weaver {
	
	/*================================================================================================*/
	public class WeaverVarAlias : IWeaverVarAlias {

		public string Name { get { return vName+""; } }
		
		private readonly string vName;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverVarAlias(IWeaverTransaction pCurrentTx) {
			vName = pCurrentTx.GetNextVarName();
		}

	}

}