using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public static class WeaverTasks {

		public enum ItemType {
			Node,
			Rel
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPath<T> BeginPath<T>(T pBaseNode) where T : class, IWeaverItem, new() {
			return new WeaverPath<T>(new WeaverQuery(), pBaseNode);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPathFromManualIndex<T> BeginPath<T>(string pIndexName,
											Expression<Func<T, object>> pProp, object pValue)
													where T : class, IWeaverItemIndexable, new() {
			return new WeaverPathFromManualIndex<T>(new WeaverQuery(), pIndexName,pProp,pValue);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPathFromKeyIndex<T> BeginPath<T>(Expression<Func<T, object>> pProp,
										object pValue) where T : class, IWeaverItemIndexable, new() {
			return new WeaverPathFromKeyIndex<T>(new WeaverQuery(), pProp, pValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddNode<T>(T pNode) where T : IWeaverItem {
			var q = new WeaverQuery();
			q.FinalizeQuery("g.addVertex(["+WeaverUtil.BuildPropList(q, pNode)+"])");
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery CreateManualIndex(string pIndexName, ItemType pType) {
			var q = new WeaverQuery();
			var nameVal = new WeaverQueryVal(pIndexName, false);
			string type = (pType == ItemType.Rel ? "Edge" : "Vertex");
			q.FinalizeQuery("g.createManualIndex("+q.AddParam(nameVal)+","+type+".class)");
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery CreateKeyIndex<T>(Expression<Func<T, object>> pProp,
														ItemType pType) where T : IWeaverItemIndexable {
			var q = new WeaverQuery();
			var nameVal = new WeaverQueryVal(WeaverUtil.GetPropertyName(pProp), false);
			string type = (pType == ItemType.Rel ? "Edge" : "Vertex");
			q.FinalizeQuery("g.createKeyIndex("+q.AddParam(nameVal)+","+type+".class)");
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
		public static IWeaverQuery AddNodeToIndex<T>(string pIndexName, IWeaverVarAlias<T> pVar,
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
			if ( pRel.FromNodeType != pFromVar.VarType ) {
				throw new WeaverException("Invalid From VarType: '"+pFromVar.VarType.Name+
					"', expected '"+pRel.FromNodeType.Name+"'.");
			}
			
			if ( pRel.ToNodeType != pToVar.VarType ) {
				throw new WeaverException("Invalid To VarType: '"+pToVar.VarType.Name+
					"', expected '"+pRel.ToNodeType.Name+"'.");
			}
			
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