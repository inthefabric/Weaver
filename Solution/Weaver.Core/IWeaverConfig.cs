using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Core.Items;
using Weaver.Core.Schema;

namespace Weaver.Core {

	/*================================================================================================*/
	public interface IWeaverConfig {

		IList<WeaverVertexSchema> VertexSchemas { get; }
		IList<WeaverEdgeSchema> EdgeSchemas { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string GetItemDbName<T>(T pItem) where T : IWeaverItemIndexable;
		string GetItemDbName<T>() where T : IWeaverItemIndexable;
		string GetItemDbName(string pItemName);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		string GetPropertyDbName<T>(IWeaverFunc pFunc, Expression<Func<T, object>> pExp)
																		where T : IWeaverItemIndexable;

		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyDbName<T>(Expression<Func<T, object>> pExp) where T : IWeaverItemIndexable;
		
		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyDbName<T>(string pProp) where T : IWeaverItemIndexable;

	}

}