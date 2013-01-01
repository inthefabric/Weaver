using Weaver.Interfaces;

namespace Weaver {
	
	/*================================================================================================*/
	public class WeaverListVar : IWeaverListVar {

		public string Name { get { return vName+""; } }
		
		private readonly string vName;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverListVar(IWeaverTransaction pCurrentTx) {
			vName = pCurrentTx.GetNextVarName();
		}

	}

}