using System;
using Weaver.Core.Elements;

namespace Weaver.Core.Query {
	
	/*================================================================================================*/
	public class WeaverVarAlias : IWeaverVarAlias {

		public string Name { get; set; }
		public Type VarType { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverVarAlias(IWeaverTransaction pCurrentTx) {
			Name = pCurrentTx.GetNextVarName();
			VarType = typeof(object);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public WeaverVarAlias(IWeaverTransaction pCurrentTx, Type pVarType) : this(pCurrentTx) {
			VarType = pVarType;
		}

	}
	
	
	/*================================================================================================*/
	public class WeaverVarAlias<T> : WeaverVarAlias, IWeaverVarAlias<T> where T : IWeaverElement {
		
		
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