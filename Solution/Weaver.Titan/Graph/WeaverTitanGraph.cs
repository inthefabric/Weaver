using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

			var keys = new List<string>();
			var sigs = new List<string>();

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