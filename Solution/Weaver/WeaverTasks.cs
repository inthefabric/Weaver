using System;
using System.Linq.Expressions;
using System.Reflection;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver {

	/*================================================================================================*/
	public static class WeaverTasks {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPath<T> BeginPath<T>(T pBaseNode) where T : class, IWeaverItem, new() {
			return new WeaverPath<T>(new WeaverQuery(), pBaseNode);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPath<T> BeginPath<T>(string pIndexName, Expression<Func<T, object>> pFunc,
										object pValue) where T : class, IWeaverIndexableItem, new() {
			return new WeaverPathFromIndex<T>(new WeaverQuery(), pIndexName, pFunc, pValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery AddNode<T>(T pNode) where T : IWeaverItem {
			var q = new WeaverQuery();
			q.FinalizeQuery("g.addVertex(["+BuildPropList(q, pNode)+"]);");
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
			q.FinalizeQuery("g.createManualIndex("+q.AddParam(nameVal)+","+type+".class);");
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
			var propNameVal = new WeaverQueryVal(WeaverFuncProp.GetPropertyName(pFunc), false);
			var propValVal = new WeaverQueryVal(pFunc.Compile()(pNode));

			q.FinalizeQuery("n=g.v("+nodeIdVal.FixedText+");g.idx("+q.AddParam(indexNameVal)+").put("+
				q.AddParam(propNameVal)+","+q.AddParamIfString(propValVal)+",n);");
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

			string propList = BuildPropList(q, pRel);
			script += (propList.Length > 0 ? ",["+propList+"]" : "")+");";

			q.FinalizeQuery(script);
			return q;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string BuildPropList<TItem>(IWeaverQuery pQuery, TItem pItem,
								bool pIncludeId=false, int pStartParamI=0) where TItem : IWeaverItem {
			string list = "";
			int i = pStartParamI;

			foreach ( PropertyInfo prop in pItem.GetType().GetProperties() ) {
				object[] propAtts = prop.GetCustomAttributes(typeof(WeaverItemPropertyAttribute), true);
				if ( propAtts.Length == 0 ) { continue; }
				if ( !pIncludeId && prop.Name == "Id" ) { continue; }
				object val = prop.GetValue(pItem, null);
				if ( val == null ) { continue; }
				if ( i++ > 0 ) { list += ","; }

				var valVal = new WeaverQueryVal(val, false);
				list += prop.Name+":"+pQuery.AddParamIfString(valVal);
			}

			return list;
		}

	}

}