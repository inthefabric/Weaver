using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverQuery {

		string Script { get; }
		Dictionary<string, string> Params { get; }
		WeaverQuery.ResultQuantity ExpectQuantity { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverPath<TBase> BeginPath<TBase>(TBase pBaseNode) where TBase : class, IWeaverItem, new();
		IWeaverPath<T> BeginPathWithIndex<T>(string pIndexName, Expression<Func<T, object>> pFunc,
													object pValue) where T : class, IWeaverNode, new();
		void FinishPathWithQuantity(WeaverQuery.ResultQuantity pQuantity);
		void FinishPathWithUpdate<T>(WeaverUpdates<T> pUpdates) where T : IWeaverNode;

		/*--------------------------------------------------------------------------------------------*/
		void AddParam(string pParamName, WeaverQueryVal pValue);
		string AddParam(WeaverQueryVal pValue);
		string AddParamIfString(WeaverQueryVal pValue);
		string NextParamName { get; }

	}

}