using System;
using Weaver.Core.Elements;

namespace Weaver.Core.Query {
	
	/*================================================================================================*/
	public interface IWeaverVarAlias {

		string Name { get; }
		Type VarType { get; }

	}

	
	/*================================================================================================*/
	public interface IWeaverVarAlias<T> : IWeaverVarAlias where T : IWeaverElement {
		
	}

}