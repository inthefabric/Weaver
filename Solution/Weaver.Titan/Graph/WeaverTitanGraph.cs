using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Graph;
using Weaver.Core.Pipe;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Core.Util;
using Weaver.Titan.Elements;

namespace Weaver.Titan.Graph {
	
	/*================================================================================================*/
	public class WeaverTitanGraph : WeaverGraph, IWeaverTitanGraph {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverTitanGraphQuery QueryV() {
			var v = new WeaverTitanGraphQuery(true);
			Path.AddItem(v);
			return v;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverTitanGraphQuery QueryE() {
			var v = new WeaverTitanGraphQuery(false);
			Path.AddItem(v);
			return v;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery AddEdgeVci<TOut, TEdge, TIn>(TOut pOutVertex, TEdge pEdge, TIn pInVertex)
						where TOut : IWeaverVertex where TEdge : IWeaverEdge where TIn : IWeaverVertex {
			if ( pOutVertex.Id == null ) {
				throw new WeaverException("OutVertex.Id cannot be null.");
			}

			if ( pInVertex.Id == null ) {
				throw new WeaverException("InVertex.Id cannot be null.");
			}

			const string outV = "_A";
			const string inV = "_B";

			return FinishEdgeVci(pEdge, outV, inV,
				outV+"=g.v("+Path.Query.AddStringParam(pOutVertex.Id)+");"+
				inV+"=g.v("+Path.Query.AddStringParam(pInVertex.Id)+");"
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery AddEdgeVci<TEdge>(IWeaverVarAlias pOutVertexVar, TEdge pEdge,
											IWeaverVarAlias pInVertexVar) where TEdge : IWeaverEdge {
			if ( !pEdge.IsValidOutVertexType(pOutVertexVar.VarType) ) {
				throw new WeaverException("Invalid Out VarType: '"+pOutVertexVar.VarType.Name+
					"', expected '"+pEdge.OutVertexType.Name+"'.");
			}

			if ( !pEdge.IsValidInVertexType(pInVertexVar.VarType) ) {
				throw new WeaverException("Invalid In VarType: '"+pInVertexVar.VarType.Name+
					"', expected '"+pEdge.InVertexType.Name+"'.");
			}

			return FinishEdgeVci(pEdge, pOutVertexVar.Name, pInVertexVar.Name, "");
		}

		/*--------------------------------------------------------------------------------------------*/
		private IWeaverQuery FinishEdgeVci<TEdge>(TEdge pEdge, string pOutV, string pInV,
															string pScript) where TEdge : IWeaverEdge {
			Type et = typeof(TEdge);
			var e = GetAndVerifyElementAttribute<WeaverTitanEdgeAttribute>(et);

			string labelParam = Path.Query.AddStringParam(e.DbName);
			string propList = WeaverUtil.BuildPropList(Path.Config, Path.Query, pEdge);

			var sb = new StringBuilder();
			AppendEdgeVciProps(sb, pOutV, et, WeaverUtil.GetElementPropertyAttributes(e.OutVertex));
			AppendEdgeVciProps(sb, pInV, et, WeaverUtil.GetElementPropertyAttributes(e.InVertex));
			
			const string tryLoop = "_TRY.each{k,v->if((z=v.getProperty(k))){_PROP.put(k,z)}};";
			bool showTry = (sb.Length > 0);
			string propLine = (propList.Length > 0 || showTry ?
				"_PROP="+(propList.Length > 0 ? "["+propList+"];" : "[:];") : "");
			
			Path.Query.FinalizeQuery(
				pScript+
				propLine+
				(showTry ? "_TRY=["+sb+"];"+tryLoop : "")+
				"g.addEdge("+pOutV+","+pInV+","+labelParam+(propLine.Length > 0 ? ",_PROP" : "")+")"
			);

			return Path.Query;
		}


		/*--------------------------------------------------------------------------------------------*/
		private void AppendEdgeVciProps(StringBuilder pStr, string pAlias, Type pEdgeType,
																IEnumerable<WeaverPropPair> pProps) {
			foreach ( WeaverPropPair wpp in pProps ) {
				WeaverTitanPropertyAttribute att = GetAndVerifyTitanPropertyAttribute(wpp, true);

				if ( att == null || !att.HasTitanVertexCentricIndex(pEdgeType) ) {
					continue;
				}

				pStr.Append((pStr.Length > 0 ? "," : "")+att.DbName+":"+pAlias);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery TypeGroupOf<T>(int pId) where T : IWeaverVertex {
			if ( pId < 2 ) {
				throw new WeaverException("Group ID must be greater than 1.");
			}

			GetAndVerifyElementAttribute<WeaverVertexAttribute>(typeof(T));
			var q = new WeaverQuery();

			q.FinalizeQuery(
				"com.thinkaurelius.titan.core.TypeGroup.of("+
				q.AddParam(new WeaverQueryVal(pId))+",'group')"
			);

			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd MakeVertexPropertyKey<T>(Expression<Func<T, object>> pProperty,
											IWeaverVarAlias pGroupVar=null) where T : IWeaverVertex {
			GetAndVerifyElementAttribute<WeaverTitanVertexAttribute>(typeof(T));
			return MakePropertyKey("Vertex", pProperty, pGroupVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd MakeEdgePropertyKey<T>(Expression<Func<T, object>> pProperty,
												IWeaverVarAlias pGroupVar=null) where T : IWeaverEdge {
			GetAndVerifyElementAttribute<WeaverTitanEdgeAttribute>(typeof(T));
			return MakePropertyKey("Edge", pProperty, pGroupVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		private IWeaverPathPipeEnd MakePropertyKey<T>(string pElement, Expression<Func<T,object>> pProp,
											IWeaverVarAlias pGroupVar=null) where T : IWeaverElement {
			WeaverPropPair wpp = WeaverUtil.GetPropertyAttribute(pProp);
			WeaverTitanPropertyAttribute att = GetAndVerifyTitanPropertyAttribute(wpp);
			Type pt = wpp.Info.PropertyType;

			AddCustom("makeType()");
			AddCustom("dataType("+WeaverTitanPropertyAttribute.GetTitanTypeName(pt)+".class)");
			AddCustom("name("+Path.Query.AddParam(new WeaverQueryVal(att.DbName))+")");
			AddCustom("unique(OUT)"); //WeaverConfig enforces unique property DbNames

			if ( pGroupVar != null ) {
				AddCustom("group("+pGroupVar.Name+")");
			}

			if ( att.TitanIndex ) {
				AddCustom("indexed("+pElement+".class)");
			}

			if ( att.TitanElasticIndex ) {
				AddCustom("indexed('search',"+pElement+".class)");
			}

			return AddCustom("makePropertyKey()");
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd BuildEdgeLabel<T>(
						Func<string, IWeaverVarAlias> pGetPropVarAliasByDbName) where T : IWeaverEdge {
			Type et = typeof(T);
			var e = GetAndVerifyElementAttribute<WeaverTitanEdgeAttribute>(et);
			var ivc = e.InConn;
			var ovc = e.OutConn;

			var props = new List<WeaverPropPair>();
			props.AddRange(WeaverUtil.GetElementPropertyAttributes(e.OutVertex));
			props.AddRange(WeaverUtil.GetElementPropertyAttributes(e.InVertex));

			var keys = new HashSet<string>();
			var sigs = new HashSet<string>();

			foreach ( WeaverPropPair wpp in props ) {
				WeaverTitanPropertyAttribute att = GetAndVerifyTitanPropertyAttribute(wpp, true);

				if ( att == null ) {
					continue;
				}

				string alias = pGetPropVarAliasByDbName(att.DbName).Name;

				if ( att.HasTitanVertexCentricIndex(et) ) {
					keys.Add(alias);
					continue;
				}

				sigs.Add(alias);
			}

			////

			AddCustom("makeType()");
			AddCustom("name("+Path.Query.AddParam(new WeaverQueryVal(e.DbName))+")");
			
			if ( keys.Count > 0 ) {
				AddCustom("primaryKey("+string.Join(",", keys)+")");
			}

			if ( sigs.Count > 0 ) {
				AddCustom("signature("+string.Join(",", sigs)+")");
			}

			// An edge label is out-unique, if a vertex has at most one outgoing edge for that label. 
			// "father" is of an out-unique edge label, since each god has at most one father.
			// https://github.com/thinkaurelius/titan/wiki/Type-Definition-Overview

			if ( ivc == WeaverEdgeConn.InOne || ivc == WeaverEdgeConn.InZeroOrOne ) {
				AddCustom("unique(IN)");
			}

			if ( ovc == WeaverEdgeConn.OutOne || ovc == WeaverEdgeConn.OutZeroOrOne ) {
				AddCustom("unique(OUT)");
			}

			return AddCustom("makeEdgeLabel()");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private WeaverStepCustom AddCustom(string pScript) {
			var sc = new WeaverStepCustom(pScript);
			Path.AddItem(sc);
			return sc;
		}

		/*--------------------------------------------------------------------------------------------*/
		private T GetAndVerifyElementAttribute<T>(Type pType) where T : WeaverElementAttribute {
			T att = WeaverUtil.GetElementAttribute<T>(pType);

			if ( att == null ) {
				throw new WeaverException("Type '"+pType.Name+"' must have a "+typeof(T).Name+".");
			}

			return att;
		}

		/*--------------------------------------------------------------------------------------------*/
		private WeaverTitanPropertyAttribute GetAndVerifyTitanPropertyAttribute(
													WeaverPropPair pPair, bool pIgnoreNonTitan=false) {
			WeaverTitanPropertyAttribute a = (pPair.Attrib as WeaverTitanPropertyAttribute);

			if ( !pIgnoreNonTitan && a == null ) {
				throw new WeaverException("Type '"+pPair.Info.Name+"' must have a "+
					typeof(WeaverTitanPropertyAttribute).Name+".");
			}

			return a;
		}

	}

}