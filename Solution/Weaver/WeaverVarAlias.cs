using Weaver.Interfaces;
using System;

namespace Weaver {
	
	/*================================================================================================*/
	public class WeaverVarAlias : IWeaverVarAlias {

		public string Name { get { return vName+""; } }
		public Type VarType { get; private set; }
		
		private readonly string vName;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverVarAlias(IWeaverTransaction pCurrentTx) {
			vName = pCurrentTx.GetNextVarName();
			VarType = typeof(object);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public WeaverVarAlias(IWeaverTransaction pCurrentTx, Type pVarType) : this(pCurrentTx) {
			VarType = pVarType;
		}

	}
	
	
	/*================================================================================================*/
	public class WeaverVarAlias<T> : WeaverVarAlias, IWeaverVarAlias<T> where T : IWeaverItemIndexable {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverVarAlias(IWeaverTransaction pCurrentTx) : base(pCurrentTx, typeof(T)) {}
		
	}


	/*================================================================================================*/
	public class WeaverTableVarAlias : WeaverVarAlias, IWeaverTableVarAlias {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverTableVarAlias(IWeaverTransaction pCurrentTx) : base(pCurrentTx) {}

	}

}