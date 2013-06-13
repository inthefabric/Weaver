using System;
using Weaver.Core.Elements;

namespace Weaver.Core.Query {
	
	/*================================================================================================*/
	public class WeaverVarAlias : IWeaverVarAlias {

		public string Name { get; private set; }
		public Type VarType { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverVarAlias(string pName, Type pVarType=null) {
			Name = pName;
			VarType = (pVarType ?? typeof(object));
		}

	}
	
	
	/*================================================================================================*/
	public class WeaverVarAlias<T> : WeaverVarAlias, IWeaverVarAlias<T> where T : IWeaverElement {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverVarAlias(string pName) : base(pName, typeof(T)) {}
		
	}

}