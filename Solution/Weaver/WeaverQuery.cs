using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverQuery : IWeaverQuery {

		public string Script { get; set; }
		public Dictionary<string, string> Params { get; set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQuery(string pScript) {
			Script = pScript;
			Params = new Dictionary<string, string>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQuery() : this("") {}

		/*--------------------------------------------------------------------------------------------*/
		public void AddParam(string pParamName, WeaverQueryVal pValue) {
			Params.Add(pParamName, pValue.GetQuoted());
		}

		/*--------------------------------------------------------------------------------------------*/
		public string AddParam(WeaverQueryVal pValue) {
			string p = NextParamName;
			AddParam(p, pValue);
			return p;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string AddParamIfString(WeaverQueryVal pValue) {
			if ( !pValue.IsString ) { return pValue.FixedText; }
			return AddParam(pValue);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string NextParamName {
			get { return "_P"+Params.Keys.Count; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static WeaverQuery AddNode<T>(T pNode) where T : IWeaverItem {
			var q = new WeaverQuery();
			q.Script = "g.addVertex(["+BuildPropList(q, pNode)+"]);";
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static WeaverQuery AddNodeIndex(string pIndexName) {
			var q = new WeaverQuery();
			var nameVal = new WeaverQueryVal(pIndexName, false);
			q.Script = "g.createManualIndex("+q.AddParam(nameVal)+",Vertex.class);";
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static WeaverQuery AddNodeToIndex<T>(string pIndexName, T pNode,
											Expression<Func<T,object>> pFunc) where T : IWeaverNode {
			if ( pNode.Id < 0 ) {
				throw new WeaverException("Node.Id cannot be less than zero: "+pNode.Id);
			}

			var q = new WeaverQuery();
			
			var nodeIdVal = new WeaverQueryVal(pNode.Id);
			var indexNameVal = new WeaverQueryVal(pIndexName, false);
			var propNameVal = new WeaverQueryVal(WeaverFuncProp.GetPropertyName(pFunc));
			var propValVal = new WeaverQueryVal(pFunc.Compile()(pNode));

			q.Script = "n=g.v("+nodeIdVal.FixedText+");g.idx("+q.AddParam(indexNameVal)+").put("+
				q.AddParam(propNameVal)+","+q.AddParamIfString(propValVal)+",n);";

			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static WeaverQuery UpdateNodesAtPath<T>(IWeaverPath pPath, WeaverUpdates<T> pUpdates)
																				where T : IWeaverNode {
			var q = new WeaverQuery();
			q.Script = pPath.GremlinCode+".each{";

			for ( int i = 0 ; i < pUpdates.Count ; ++i ) {
				KeyValuePair<string, WeaverQueryVal> pair = pUpdates[i];
				q.Script += (i == 0 ? "" : ";")+"it."+pair.Key+"="+q.AddParamIfString(pair.Value);
			}

			q.Script += "};";
			return q;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static WeaverQuery AddRel<TFrom, TRel, TTo>(TFrom pFromNode, TRel pRel, TTo pToNode)
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

			q.Script = "f=g.v("+fromNodeVal.FixedText+");t=g.v("+toNodeVal.FixedText+");"+
				"g.addEdge(f,t,"+q.AddParam(relLabelVal);

			string propList = BuildPropList(q, pRel);
			q.Script += (propList.Length > 0 ? ",["+propList+"]" : "")+");";

			return q;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string QuoteValueIfString(object pValue) {
			return (pValue is string ? QuoteValue(pValue) : pValue+"");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static string QuoteValue(object pValue) {
			return "'"+pValue+"'";
		}

		/*--------------------------------------------------------------------------------------------* /
		public static string FixValue(object pValue) {
			if ( pValue is bool ) { return (pValue+"").ToLower(); }
			return pValue;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static string BuildPropList<TItem>(IWeaverQuery pQuery, TItem pItem,
													bool pIncludeId=false) where TItem : IWeaverItem {
			string list = "";
			int i = 0;

			foreach ( PropertyInfo prop in pItem.GetType().GetProperties() ) {
				object[] propAtts = prop.GetCustomAttributes(typeof(WeaverItemPropertyAttribute), true);
				if ( propAtts.Length == 0 ) { continue; }
				if ( !pIncludeId && prop.Name == "Id" ) { continue; }
				object val = prop.GetValue(pItem, null);
				if ( val == null ) { continue; }
				if ( i++ > 0 ) { list += ","; }

				var valVal = new WeaverQueryVal(val);
				list += prop.Name+":"+pQuery.AddParamIfString(valVal);
			}

			return list;
		}

	}

}