﻿using System;

namespace Weaver.Interfaces {
	
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