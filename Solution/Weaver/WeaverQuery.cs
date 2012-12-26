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

		public enum ResultQuantity {
			Single = 1,
			List
		}

		public string Script { get; private set; }
		public Dictionary<string, string> Params { get; private set; }
		public ResultQuantity? ExpectQuantity { get; private set; } //TEST: WeaverQuery.ExpectQuantity

		private IWeaverPath vPath;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQuery(string pScript, Dictionary<string, string> pParams=null) {
			Script = pScript;
			Params = (pParams ?? new Dictionary<string, string>());
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQuery() : this("") {}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverQuery.BeginPath
		public WeaverPath<TBase> BeginPath<TBase>(TBase pBaseNode)
															where TBase : class, IWeaverItem, new() {
			if ( Script != null ) {
				throw new WeaverException("Script is already set.");
			}

			var wp = new WeaverPath<TBase>(this, pBaseNode);
			vPath = wp;
			return wp;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverQuery.BeginPathWithIndex
		public WeaverPath<T> BeginPathWithIndex<T>(string pIndexName, Expression<Func<T, object>> pFunc,
												object pValue) where T : class, IWeaverNode, new() {
			if ( Script != null ) {
				throw new WeaverException("Script is already set.");
			}

			var p = new WeaverPath<T>(this);
			p.StartWithIndex(pIndexName, pFunc, pValue);
			vPath = p;
			return p;
		}

		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverQuery.FinishPathWithQuantity
		public void FinishPathWithQuantity(ResultQuantity pQuantity) {
			if ( vPath == null ) {
				throw new WeaverException("Path is null.");
			}

			ExpectQuantity = pQuantity;
			Script = vPath.GetParameterizedScriptAndFinish();
		}

		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverQuery.FinishPathWithUpdate
		public void FinishPathWithUpdate<T>(WeaverUpdates<T> pUpdates) where T : IWeaverNode {
			if ( vPath == null ) {
				throw new WeaverException("Path is null.");
			}

			Script = vPath.GetParameterizedScriptAndFinish()+".each{";

			for ( int i = 0 ; i < pUpdates.Count ; ++i ) {
				KeyValuePair<string, WeaverQueryVal> pair = pUpdates[i];
				Script += (i == 0 ? "" : ";")+"it."+pair.Key+"="+AddParamIfString(pair.Value);
			}
			
			Script += "};";
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
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
			var propNameVal = new WeaverQueryVal(WeaverFuncProp.GetPropertyName(pFunc), false);
			var propValVal = new WeaverQueryVal(pFunc.Compile()(pNode));

			q.Script = "n=g.v("+nodeIdVal.FixedText+");g.idx("+q.AddParam(indexNameVal)+").put("+
				q.AddParam(propNameVal)+","+q.AddParamIfString(propValVal)+",n);";

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