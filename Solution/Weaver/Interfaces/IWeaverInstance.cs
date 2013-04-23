using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverInstance {

		WeaverConfig Config { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverTransaction NewTx();
		IWeaverQuery NewQuery();
		WeaverUpdates<T> NewUpdates<T>() where T : IWeaverItemIndexable;
		WeaverTableColumns NewTableColumns();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverPath<T> BeginPath<T>(T pBaseNode) where T : class, IWeaverItem, new();

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathFromNodeId<T> BeginPath<T>(string pNodeId)
			where T : class, IWeaverItemIndexable, new();

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathFromVarAlias<T> BeginPath<T>(IWeaverVarAlias<T> pBaseNodeAlias,
							bool pCopyItemIntoVar=false) where T : class, IWeaverItemIndexable, new();

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathFromKeyIndex<T> BeginPath<T>(Expression<Func<T, object>> pProp,
											object pValue) where T : class, IWeaverItemIndexable, new();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddNode<T>(T pNode, bool pIncludeNulls=false) where T : IWeaverNode;

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddRel<TFrom, TRel, TTo>(TFrom pFromNode, TRel pRel, TTo pToNode)
			where TFrom : IWeaverNode
			where TRel : IWeaverRel
			where TTo : IWeaverNode;

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery AddRel<TRel>(IWeaverVarAlias pFromVar, TRel pRel,
												  IWeaverVarAlias pToVar) where TRel : IWeaverRel;

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery FinishRel<TRel>(IWeaverQuery pQuery, TRel pRel,
															 string pScript) where TRel : IWeaverRel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery InitListVar(IWeaverTransaction pCurrentTx,
												IList<IWeaverVarAlias> pVars, out IWeaverVarAlias pVar);

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery InitListVar(IWeaverTransaction pCurrentTx, out IWeaverVarAlias pVar);

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery InitTableVar(IWeaverTransaction pCurrentTx, out IWeaverTableVarAlias pVar);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery StoreQueryResultAsVar(IWeaverTransaction pCurrentTx,
														IWeaverQuery pQuery, out IWeaverVarAlias pVar);

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery StoreQueryResultAsVar<T>(IWeaverTransaction pCurrentTx,
					IWeaverQuery pQuery, out IWeaverVarAlias<T> pVar) where T : IWeaverItemIndexable;

	}

}