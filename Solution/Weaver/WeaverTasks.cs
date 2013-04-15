using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	internal static class WeaverTasks {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPath<T> BeginPath<T>(IWeaverConfig pConfig, T pBaseNode) where T : class, IWeaverItem, new() {
			return new WeaverPath<T>(pConfig, new WeaverQuery(), pBaseNode);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPathFromNodeId<T> BeginPath<T>(IWeaverConfig pConfig, string pNodeId) 
														where T : class, IWeaverItemIndexable, new() {
			return new WeaverPathFromNodeId<T>(pConfig, new WeaverQuery(), pNodeId);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPathFromVarAlias<T> BeginPath<T>(IWeaverConfig pConfig, 
										IWeaverVarAlias<T> pBaseNodeAlias, bool pCopyItemIntoVar=false)
										where T : class, IWeaverItemIndexable, new() {
			return new WeaverPathFromVarAlias<T>(
				pConfig, new WeaverQuery(), pBaseNodeAlias, pCopyItemIntoVar);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPathFromKeyIndex<T> BeginPath<T>(IWeaverConfig pConfig, 
													Expression<Func<T, object>> pProp, object pValue)
													where T : class, IWeaverItemIndexable, new() {
			return new WeaverPathFromKeyIndex<T>(pConfig, new WeaverQuery(), pProp, pValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddNode<T>(IWeaverConfig pConfig, T pNode,
													bool pIncludeNulls=false) where T : IWeaverNode {
			var q = new WeaverQuery();
			string props = WeaverUtil.BuildPropList(pConfig, q, pNode, false, 0, pIncludeNulls);
			q.FinalizeQuery("g.addVertex(["+props+"])");
			return q;
		}

		/*--------------------------------------------------------------------------------------------* /
		public static IWeaverQuery CreateKeyIndex<T>(Expression<Func<T, object>> pProp,
														ItemType pType) where T : IWeaverItemIndexable {
			var q = new WeaverQuery();
			string type = (pType == ItemType.Rel ? "Edge" : "Vertex");
			q.FinalizeQuery("g.createKeyIndex("+
				q.AddStringParam(WeaverUtil.GetPropertyName(pProp))+","+type+".class)");
			return q;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddRel<TFrom, TRel, TTo>(IWeaverConfig pConfig, TFrom pFromNode,
													TRel pRel, TTo pToNode) where TFrom : IWeaverNode 
													where TRel : IWeaverRel where TTo : IWeaverNode {
			if ( pFromNode.Id == null ) {
				throw new WeaverException("FromNode.Id cannot be null.");
			}

			if ( pToNode.Id == null ) {
				throw new WeaverException("ToNode.Id cannot be null.");
			}

			var q = new WeaverQuery();

			string script = "f=g.v("+q.AddStringParam(pFromNode.Id)+");"+
				"t=g.v("+q.AddStringParam(pToNode.Id)+");"+
				"g.addEdge(f,t,"+q.AddStringParam(pRel.Label);

			return FinishRel(pConfig, q, pRel, script);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddRel<TRel>(IWeaverConfig pConfig, IWeaverVarAlias pFromVar,
											TRel pRel, IWeaverVarAlias pToVar) where TRel : IWeaverRel {
			if ( !pRel.FromNodeType.IsAssignableFrom(pFromVar.VarType) ) {
				throw new WeaverException("Invalid From VarType: '"+pFromVar.VarType.Name+
					"', expected '"+pRel.FromNodeType.Name+"'.");
			}

			if ( !pRel.ToNodeType.IsAssignableFrom(pToVar.VarType) ) {
				throw new WeaverException("Invalid To VarType: '"+pToVar.VarType.Name+
					"', expected '"+pRel.ToNodeType.Name+"'.");
			}
			
			var q = new WeaverQuery();
			string script = "g.addEdge("+pFromVar.Name+","+pToVar.Name+","+
				q.AddStringParam(pRel.Label);
			return FinishRel(pConfig, q, pRel, script);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery FinishRel<TRel>(IWeaverConfig pConfig, IWeaverQuery pQuery,
													TRel pRel, string pScript) where TRel : IWeaverRel {
			string propList = WeaverUtil.BuildPropList(pConfig, pQuery, pRel);
			pScript += (propList.Length > 0 ? ",["+propList+"]" : "")+")";

			pQuery.FinalizeQuery(pScript);
			return pQuery;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery InitListVar(IWeaverTransaction pCurrentTx,
											IList<IWeaverVarAlias> pVars, out IWeaverVarAlias pVar) {
			pVar = new WeaverVarAlias(pCurrentTx);
			
			string list = "";

			foreach ( IWeaverVarAlias var in pVars ) {
				list += (list == "" ? "" : ",")+var.Name;
			}

			var q = new WeaverQuery();
			q.FinalizeQuery(pVar.Name+"=["+list+"]");
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery InitListVar(IWeaverTransaction pCurrentTx, out IWeaverVarAlias pVar){
			return InitListVar(pCurrentTx, new List<IWeaverVarAlias>(), out pVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverTasks.InitTableVar()
		public static IWeaverQuery InitTableVar(IWeaverTransaction pCurrentTx,
																		out IWeaverTableVarAlias pVar) {
			pVar = new WeaverTableVarAlias(pCurrentTx);

			var q = new WeaverQuery();
			q.FinalizeQuery(pVar.Name+"=new Table()");
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery StoreQueryResultAsVar(IWeaverTransaction pCurrentTx,
														IWeaverQuery pQuery, out IWeaverVarAlias pVar) {
			pVar = new WeaverVarAlias(pCurrentTx);
			pQuery.StoreResultAsVar(pVar);
			return pQuery;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverTasks.StoreQueryResultAsVar()
		public static IWeaverQuery StoreQueryResultAsVar<T>(IWeaverTransaction pCurrentTx,
					IWeaverQuery pQuery, out IWeaverVarAlias<T> pVar) where T : IWeaverItemIndexable {
			pVar = new WeaverVarAlias<T>(pCurrentTx);
			pQuery.StoreResultAsVar(pVar);
			return pQuery;
		}

	}

}