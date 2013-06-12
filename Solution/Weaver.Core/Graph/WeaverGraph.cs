﻿using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Util;

namespace Weaver.Core.Graph {
	
	/*================================================================================================*/
	public class WeaverGraph : WeaverPathItem, IWeaverGraph {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverAllVertices V {
			get {
				var v = new WeaverAllVertices();
				Path.AddItem(v);
				return v;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverAllEdges E {
			get {
				var e = new WeaverAllEdges();
				Path.AddItem(e);
				return e;
			}
		}

		//TODO: T WeaverGraph.Vertex<T>(string pVertexId)
		//TODO: T WeaverGraph.Edge<T>(string pEdgeId)


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery AddVertex<T>(T pVertex) where T : IWeaverVertex {
			var q = new WeaverQuery();
			string props = WeaverUtil.BuildPropList(Path.Config, q, pVertex);
			q.FinalizeQuery("g.addVertex(["+props+"])");
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery AddEdge<TOut, TEdge, TIn>(TOut pOutVertex, TEdge pEdge, TIn pInVertex)
						where TOut : IWeaverVertex where TEdge : IWeaverEdge where TIn : IWeaverVertex {
			if ( pOutVertex.Id == null ) {
				throw new WeaverException("FromNode.Id cannot be null.");
			}

			if ( pInVertex.Id == null ) {
				throw new WeaverException("ToNode.Id cannot be null.");
			}

			return FinishEdge(pEdge, "g.addEdge("+
				"g.v("+Path.Query.AddStringParam(pOutVertex.Id)+"),"+
				"g.v("+Path.Query.AddStringParam(pInVertex.Id)+"),"
			);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery AddEdge<TEdge>(IWeaverVarAlias pOutVertexVar, TEdge pEdge,
											IWeaverVarAlias pInVertexVar) where TEdge : IWeaverEdge {
			if ( !pEdge.FromVertexType.IsAssignableFrom(pOutVertexVar.VarType) ) {
				throw new WeaverException("Invalid From VarType: '"+pOutVertexVar.VarType.Name+
					"', expected '"+pEdge.FromVertexType.Name+"'.");
			}

			if ( !pEdge.ToVertexType.IsAssignableFrom(pInVertexVar.VarType) ) {
				throw new WeaverException("Invalid To VarType: '"+pInVertexVar.VarType.Name+
					"', expected '"+pEdge.ToVertexType.Name+"'.");
			}

			return FinishEdge(pEdge, "g.addEdge("+pOutVertexVar.Name+","+pInVertexVar.Name+",");
		}

		/*--------------------------------------------------------------------------------------------*/
		private IWeaverQuery FinishEdge<TEdge>(TEdge pEdge, string pScript) where TEdge : IWeaverEdge {
			string propList = WeaverUtil.BuildPropList(Path.Config, Path.Query, pEdge);

			pScript += 
				Path.Query.AddStringParam(Path.Config.GetItemDbName<TEdge>())+
				(propList.Length > 0 ? ",["+propList+"]" : "")+")";

			Path.Query.FinalizeQuery(pScript);
			return Path.Query;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return "g";
		}

	}

}