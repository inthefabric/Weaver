using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Interfaces;
using Weaver.Schema;

namespace Weaver {
	
	/*================================================================================================*/
	public class WeaverInstance { //TEST: WeaverInstance

		public WeaverConfig Config { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverInstance(IList<WeaverNodeSchema> pNodes, IList<WeaverRelSchema> pRels) {
			Config = new WeaverConfig(pNodes, pRels);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverTransaction NewTx() {
			return new WeaverTransaction();
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery NewQuery() {
			return new WeaverQuery();
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverUpdates<T> NewUpdates<T>() where T : IWeaverItemIndexable {
			return new WeaverUpdates<T>(Config);
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverTableColumns NewTableColumns() {
			return new WeaverTableColumns(Config);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPath<T> BeginPath<T>(T pBaseNode) where T : class, IWeaverItem, new() {
			return WeaverTasks.BeginPath(Config, pBaseNode);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathFromNodeId<T> BeginPath<T>(string pNodeId)
														where T : class, IWeaverItemIndexable, new() {
			return WeaverTasks.BeginPath<T>(Config, pNodeId);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathFromVarAlias<T> BeginPath<T>(IWeaverVarAlias<T> pBaseNodeAlias,
								bool pCopyItemIntoVar=false) where T : class, IWeaverItemIndexable, new() {
			return WeaverTasks.BeginPath(Config, pBaseNodeAlias, pCopyItemIntoVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathFromKeyIndex<T> BeginPath<T>(Expression<Func<T, object>> pProp,
										object pValue) where T : class, IWeaverItemIndexable, new() {
			return WeaverTasks.BeginPath(Config, pProp, pValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery AddNode<T>(T pNode, bool pIncludeNulls=false) where T : IWeaverNode {
			return WeaverTasks.AddNode(Config, pNode, pIncludeNulls);
		}

		/*--------------------------------------------------------------------------------------------* /
		public IWeaverQuery CreateKeyIndex<T>(Expression<Func<T, object>> pProp,
										WeaverTasks.ItemType pType) where T : IWeaverItemIndexable {
			return WeaverTasks.CreateKeyIndex(pProp, pType);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery AddRel<TFrom, TRel, TTo>(TFrom pFromNode, TRel pRel, TTo pToNode)
							where TFrom : IWeaverNode where TRel : IWeaverRel where TTo : IWeaverNode {
			return WeaverTasks.AddRel(Config, pFromNode, pRel, pToNode);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery AddRel<TRel>(IWeaverVarAlias pFromVar, TRel pRel,
													IWeaverVarAlias pToVar) where TRel : IWeaverRel {
														return WeaverTasks.AddRel(Config, pFromVar, pRel, pToVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery FinishRel<TRel>(IWeaverQuery pQuery, TRel pRel,
															string pScript) where TRel : IWeaverRel {
			return WeaverTasks.FinishRel(Config, pQuery, pRel, pScript);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery InitListVar(IWeaverTransaction pCurrentTx,
											IList<IWeaverVarAlias> pVars, out IWeaverVarAlias pVar) {
			return WeaverTasks.InitListVar(pCurrentTx, pVars, out pVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery InitListVar(IWeaverTransaction pCurrentTx, out IWeaverVarAlias pVar) {
			return WeaverTasks.InitListVar(pCurrentTx, out pVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverTasks.InitTableVar()
		public IWeaverQuery InitTableVar(IWeaverTransaction pCurrentTx, out IWeaverTableVarAlias pVar) {
			return WeaverTasks.InitTableVar(pCurrentTx, out pVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery StoreQueryResultAsVar(IWeaverTransaction pCurrentTx,
														IWeaverQuery pQuery, out IWeaverVarAlias pVar) {
			return WeaverTasks.StoreQueryResultAsVar(pCurrentTx, pQuery, out pVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverTasks.StoreQueryResultAsVar()
		public IWeaverQuery StoreQueryResultAsVar<T>(IWeaverTransaction pCurrentTx,
					IWeaverQuery pQuery, out IWeaverVarAlias<T> pVar) where T : IWeaverItemIndexable {
			return WeaverTasks.StoreQueryResultAsVar(pCurrentTx, pQuery, out pVar);
		}

	}

}