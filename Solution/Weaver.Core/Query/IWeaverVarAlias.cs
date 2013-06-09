using System;
using Weaver.Core.Elements;

namespace Weaver.Core.Query {
	
	/*================================================================================================*/
	public interface IWeaverVarAlias {

		string Name { get; set; }
		Type VarType { get; }

	}

	
	/*================================================================================================*/
	public interface IWeaverVarAlias<T> : IWeaverVarAlias where T : IWeaverElement {
		
	}


	/*================================================================================================*/
	public interface IWeaverTableVarAlias : IWeaverVarAlias {

	}

}