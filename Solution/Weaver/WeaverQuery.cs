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
		public void AddParam(string pParamName, string pValue) {
			Params.Add(pParamName, pValue);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string AddParam(string pValue) {
			string p = NextParamName;
			Params.Add(p, pValue);
			return p;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string NextParamName {
			get { return "P"+Params.Keys.Count; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static WeaverQuery AddNodeGremlin<T>(T pNode) where T : IWeaverItem {
			var q = new WeaverQuery();

			string grem = "g.addVertex([";
			int i = 0;

			foreach ( PropertyInfo prop in pNode.GetType().GetProperties() ) {
				object[] propAtts = prop.GetCustomAttributes(typeof(WeaverItemPropertyAttribute), true);
				if ( propAtts.Length == 0 ) { continue; }
				object val = prop.GetValue(pNode, null);
				if ( val == null ) { continue; }
				if ( i++ > 0 ) { grem += ","; }

				string valParam = q.AddParam(QuoteValueIfString(val));
				grem += prop.Name+":"+valParam;
			}

			q.Script = grem+"]);";
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static WeaverQuery AddNodeIndex(string pIndexName) {
			var q = new WeaverQuery();
			var indexNameParam = q.AddParam(pIndexName);
			q.Script = "g.createManualIndex("+indexNameParam+", Vertex.class);";
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static WeaverQuery AddNodeToIndex<T>(string pIndexName, T pNode,
											Expression<Func<T,object>> pFunc) where T : IWeaverNode {
			if ( pNode.Id < 0 ) {
				throw new WeaverException("Node Id cannot be less than zero: "+pNode.Id);
			}

			var q = new WeaverQuery();
			string nodeIdParam = q.AddParam(pNode.Id+"");
			string indexNameParam = q.AddParam(QuoteValueIfString(pIndexName, true));
			string propName = WeaverFuncProp.GetPropertyName(pFunc);
			string propNameParam = q.AddParam(QuoteValueIfString(propName));
			string propValParam = q.AddParam(QuoteValueIfString(pFunc.Compile()(pNode)));

			q.Script = "n = g.v("+nodeIdParam+");"+
				"g.idx("+indexNameParam+").put("+propNameParam+","+propValParam+",n);";

			return q;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static WeaverQuery UpdateNodesAtPath<T>(IWeaverPath pPath, WeaverUpdates<T> pUpdates)
																				where T : IWeaverNode {
			var q = new WeaverQuery();
			q.Script = pPath.GremlinCode+".each{";

			for ( int i = 0 ; i < pUpdates.Count ; ++i ) {
				KeyValuePair<string, string> pair = pUpdates[i];
				string propValParam = q.AddParam(pair.Value);
				q.Script += (i == 0 ? "" : ";")+"it."+pair.Key+"="+propValParam;
			}

			q.Script += "};";
			return q;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string QuoteValueIfString(object pValue) {
			return QuoteValueIfString(pValue, (pValue is string));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static string QuoteValueIfString(object pValue, bool pValueIsString) {
			if ( pValueIsString ) { return "'"+pValue+"'"; }
			return pValue+"";
		}

	}

}

/*

gremlin> g.V.each{ it.something = 444; it.test = "asdf" };

gremlin> g.V.something
==> 444
==> 444

gremlin> g.V.test
==> asdf
==> asdf

gremlin> g.V.sideEffect{ it.something = 333; it.test = "aaaa" }.test
==> aaaa
==> aaaa 

*/