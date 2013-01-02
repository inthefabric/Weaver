﻿using System;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public static class WeaverTasks {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPath<T> BeginPath<T>(T pBaseNode) where T : class, IWeaverItem, new() {
			return new WeaverPath<T>(new WeaverQuery(), pBaseNode);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPathFromIndex<T> BeginPath<T>(string pIndexName, Expression<Func<T, object>> pFunc,
										object pValue) where T : class, IWeaverItemIndexable, new() {
			return new WeaverPathFromIndex<T>(new WeaverQuery(), pIndexName, pFunc, pValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddNode<T>(T pNode) where T : IWeaverItem {
			var q = new WeaverQuery();
			q.FinalizeQuery("g.addVertex(["+WeaverUtil.BuildPropList(q, pNode)+"])");
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddNodeIndex(string pIndexName) {
			return AddIndex(pIndexName, true);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddRelIndex(string pIndexName) {
			return AddIndex(pIndexName, false);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static IWeaverQuery AddIndex(string pIndexName, bool pIsNode) {
			var q = new WeaverQuery();
			var nameVal = new WeaverQueryVal(pIndexName, false);
			var type = (pIsNode ? "Vertex" : "Edge");
			q.FinalizeQuery("g.createManualIndex("+q.AddParam(nameVal)+","+type+".class)");
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddNodeToIndex<T>(string pIndexName, T pNode,
											Expression<Func<T,object>> pFunc) where T : IWeaverNode {
			if ( pNode.Id < 0 ) {
				throw new WeaverException("Node.Id cannot be less than zero: "+pNode.Id);
			}

			var q = new WeaverQuery();
			
			var nodeIdVal = new WeaverQueryVal(pNode.Id);
			var indexNameVal = new WeaverQueryVal(pIndexName, false);
			var propNameVal = new WeaverQueryVal(WeaverUtil.GetPropertyName(pFunc), false);
			var propValVal = new WeaverQueryVal(pFunc.Compile()(pNode));

			q.FinalizeQuery("n=g.v("+nodeIdVal.FixedText+");g.idx("+q.AddParam(indexNameVal)+").put("+
				q.AddParam(propNameVal)+","+q.AddParamIfString(propValVal)+",n)");
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddNodeToIndex<T>(string pIndexName, IWeaverVarAlias pVar,
											Expression<Func<T, object>> pFunc) where T : IWeaverNode {
			var q = new WeaverQuery();

			var indexNameVal = new WeaverQueryVal(pIndexName, false);
			var propNameVal = new WeaverQueryVal(WeaverUtil.GetPropertyName(pFunc), false);

			q.FinalizeQuery("g.idx("+q.AddParam(indexNameVal)+").put("+
				q.AddParam(propNameVal)+","+pVar.Name+"."+propNameVal.RawText+","+pVar.Name+")");
			return q;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddRel<TFrom, TRel, TTo>(TFrom pFromNode, TRel pRel, TTo pToNode)
							where TFrom : IWeaverNode where TRel : IWeaverRel where TTo : IWeaverNode {
			if ( pFromNode.Id < 0 ) {
				throw new WeaverException("FromNode.Id cannot be less than zero: "+pFromNode.Id);
			}

			if ( pToNode.Id < 0 ) {
				throw new WeaverException("ToNode.Id cannot be less than zero: "+pToNode.Id);
			}

			var q = new WeaverQuery();
			var fromNodeVal= new WeaverQueryVal(pFromNode.Id);
			var toNodeVal = new WeaverQueryVal(pToNode.Id);
			var relLabelVal = new WeaverQueryVal(pRel.Label, false);

			string script = "f=g.v("+fromNodeVal.FixedText+");t=g.v("+toNodeVal.FixedText+");"+
				"g.addEdge(f,t,"+q.AddParam(relLabelVal);

			return FinishRel(q, pRel, script);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddRel<TRel>(IWeaverVarAlias pFromVar, TRel pRel,
													IWeaverVarAlias pToVar) where TRel : IWeaverRel {
			var q = new WeaverQuery();
			var relLabelVal = new WeaverQueryVal(pRel.Label, false);

			string script = "g.addEdge("+pFromVar.Name+","+pToVar.Name+","+q.AddParam(relLabelVal);
			return FinishRel(q, pRel, script);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery FinishRel<TRel>(IWeaverQuery pQuery, TRel pRel,
															string pScript) where TRel : IWeaverRel {
			string propList = WeaverUtil.BuildPropList(pQuery, pRel);
			pScript += (propList.Length > 0 ? ",["+propList+"]" : "")+")";

			pQuery.FinalizeQuery(pScript);
			return pQuery;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery InitListVar(IWeaverTransaction pCurrentTx, out IWeaverVarAlias pVar){
			pVar = new WeaverVarAlias(pCurrentTx);
			
			var q = new WeaverQuery();
			q.FinalizeQuery(pVar.Name+"=[]");
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery StoreQueryResultAsVar(IWeaverTransaction pCurrentTx,
														IWeaverQuery pQuery, out IWeaverVarAlias pVar) {
			pVar = new WeaverVarAlias(pCurrentTx);
			pQuery.StoreResultAsVar(pVar);
			return pQuery;
		}

	}

}