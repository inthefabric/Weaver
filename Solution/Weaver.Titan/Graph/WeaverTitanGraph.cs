using System;
using System.Collections.Generic;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Graph;
using Weaver.Core.Pipe;
using Weaver.Core.Query;
using Weaver.Core.Schema;
using Weaver.Core.Steps;
using Weaver.Titan.Schema;

namespace Weaver.Titan.Graph {
	
	/*================================================================================================*/
	public class WeaverTitanGraph : WeaverGraph, IWeaverTitanGraph {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverTitanGraphQuery Query {
			get {
				var v = new WeaverTitanGraphQuery();
				Path.AddItem(v);
				return v;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery TypeGroupOf(WeaverVertexSchema pVertex, int pId) {
			if ( pId < 2 ) {
				throw new WeaverException("Group ID must be greater than 1.");
			}

			var q = new WeaverQuery();

			q.FinalizeQuery(
				"com.thinkaurelius.titan.core.TypeGroup.of("+
					Path.Query.AddParam(new WeaverQueryVal(pId))+","+
					Path.Query.AddParam(new WeaverQueryVal(pVertex.DbName))+
				")"
			);

			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd MakePropertyKey(WeaverVertexSchema pVertex,
									WeaverTitanPropSchema pProperty, IWeaverVarAlias pGroupVar=null) {
			return MakePropertyKey("Vertex", pProperty, pGroupVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd MakePropertyKey(WeaverEdgeSchema pEdge,
									WeaverTitanPropSchema pProperty, IWeaverVarAlias pGroupVar=null) {
			return MakePropertyKey("Edge", pProperty, pGroupVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		private IWeaverPathPipeEnd MakePropertyKey(string pElement, WeaverTitanPropSchema pProperty,
																	IWeaverVarAlias pGroupVar=null) {
			AddCustom("makeType()");
			AddCustom("dataType("+GetTitanTypeName(pProperty.Type)+".class)");
			AddCustom("name("+Path.Query.AddParam(new WeaverQueryVal(pProperty.DbName))+")");
			AddCustom("unique(OUT)"); //WeaverConfig enforces unique property DbNames

			if ( pGroupVar != null ) {
				AddCustom("group("+pGroupVar.Name+")");
			}

			if ( pProperty.TitanIndex == true ) {
				AddCustom("indexed("+pElement+".class)");
			}

			if ( pProperty.TitanElasticIndex == true ) {
				AddCustom("indexed('search',"+pElement+".class)");
			}

			return AddCustom("makePropertyKey()");
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd BuildEdgeLabel(WeaverEdgeSchema pEdge) {
			var ivc = pEdge.InVertexConn;
			var ovc = pEdge.OutVertexConn;

			var keys = new List<string>();
			var sigs = new List<string>();

			foreach ( WeaverTitanPropSchema p in pEdge.OutVertex.Props ) {
				(p.HasTitanVertexCentricIndex(pEdge) ? keys : sigs).Add(p.DbName);
			}

			foreach ( WeaverTitanPropSchema p in pEdge.InVertex.Props  ) {
				(p.HasTitanVertexCentricIndex(pEdge) ? keys : sigs).Add(p.DbName);
			}

			////

			AddCustom("makeType()");
			AddCustom("name('"+pEdge.DbName+"')");
			
			if ( keys.Count > 0 ) {
				AddCustom("primaryKey("+string.Join(",", keys)+")");
			}

			if ( sigs.Count > 0 ) {
				AddCustom("signature("+string.Join(",", sigs)+")");
			}

			if ( ivc == WeaverEdgeConn.InFromOne || ivc ==WeaverEdgeConn.InFromZeroOrOne ) {
				AddCustom("unique(IN)");
			}

			if ( ovc == WeaverEdgeConn.OutToOne || ovc ==WeaverEdgeConn.OutToZeroOrOne ) {
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
		private static string GetTitanTypeName(Type pType) {
			string n;

			switch ( pType.Name ) {
				case "Boolean":
					n = "Boolean";
					break;

				case "Byte":
					n = "Byte";
					break;

				case "Int32":
					n = "Integer";
					break;

				case "Int64":
				case "DateTime":
					n = "Long";
					break;

				case "Single":
					n = "Float";
					break;

				case "Double":
					n = "Double";
					break;

				default:
					n = pType.Name;
					break;
			}

			return n;
		}

	}

}