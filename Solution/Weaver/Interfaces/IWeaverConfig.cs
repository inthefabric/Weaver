using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Schema;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverConfig {

		IList<WeaverNodeSchema> Nodes { get; }
		IList<WeaverRelSchema> Rels { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string GetItemDbName<T>(T pItem) where T : IWeaverItemIndexable;
		string GetItemDbName<T>() where T : IWeaverItemIndexable;
		string GetItemDbName(string pItemName);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyDbName<T>(IWeaverFunc pFunc, Expression<Func<T, object>> pExp)
																		where T : IWeaverItemIndexable;

		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyDbName<T>(Expression<Func<T, object>> pExp) where T : IWeaverItemIndexable;
		
		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyDbName<T>(string pProp) where T : IWeaverItemIndexable;

	}

}