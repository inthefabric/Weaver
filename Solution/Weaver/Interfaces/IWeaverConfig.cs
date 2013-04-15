using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Schema;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverConfig {

		IList<WeaverNodeSchema> Nodes { get; }
		IList<WeaverRelSchema> Rels { get; }

		IDictionary<string, WeaverItemSchema> ItemNameMap { get; }
		IDictionary<string, WeaverPropSchema> ItemPropNameMap { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyName<T>(IWeaverFunc pFunc, Expression<Func<T, object>> pExp)
																		where T : IWeaverItemIndexable;

		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyName<T>(Expression<Func<T, object>> pExp) where T : IWeaverItemIndexable;
		
		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyName<T>(string pProp) where T : IWeaverItemIndexable;

	}

}