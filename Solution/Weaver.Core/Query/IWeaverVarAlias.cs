using System;
using Weaver.Core.Items;

namespace Weaver.Core.Query {
	
	/*================================================================================================*/
	public interface IWeaverVarAlias {

		string Name { get; set; }
		Type VarType { get; }

	}

	
	/*================================================================================================*/
	public interface IWeaverVarAlias<T> : IWeaverVarAlias where T : IWeaverItemIndexable {
		
	}


	/*================================================================================================*/
	public interface IWeaverTableVarAlias : IWeaverVarAlias {

	}

}