using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Pipe;
using Weaver.Core.Steps;
using Weaver.Core.Util;
using Weaver.Titan.Elements;
using Weaver.Titan.Util;

namespace Weaver.Titan.Pipe {
	
	/*================================================================================================*/
	public static class WeaverTitanPathPipeExt {
	

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static TEdge HasVci<TEdge, TVert>(this TEdge pEdge, 
						Expression<Func<TVert, object>> pProperty, WeaverStepHasOp pOperation,
						object pValue) where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState(pEdge, pProperty);
			NewVert<TVert>(pEdge).Has(pProperty, pOperation, pValue);
			return pEdge;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TEdge HasVci<TEdge, TVert>(this TEdge pEdge, 
										Expression<Func<TVert, object>> pProperty) 
										where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState(pEdge, pProperty);
			NewVert<TVert>(pEdge).Has(pProperty);
			return pEdge;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TEdge HasNotVci<TEdge, TVert>(this TEdge pEdge,
						Expression<Func<TVert, object>> pProperty, WeaverStepHasOp pOperation,
						object pValue) where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState(pEdge, pProperty);
			NewVert<TVert>(pEdge).HasNot(pProperty, pOperation, pValue);
			return pEdge;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TEdge HasNotVci<TEdge, TVert>(this TEdge pEdge,
										Expression<Func<TVert, object>> pProperty) 
										where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState(pEdge, pProperty);
			NewVert<TVert>(pEdge).HasNot(pProperty);
			return pEdge;
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static TVert NewVert<TVert>(IWeaverEdge pEdge) where TVert : IWeaverVertex, new() {
			TVert v = new TVert();
			v.Path = pEdge.Path;
			return v;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private static void ConfirmVciState<TEdge, TVert> (TEdge pEdge, 
												Expression<Func<TVert, object>> pProperty)
				                             	where TEdge : IWeaverEdge where TVert : IWeaverVertex {
			Type et = typeof(TEdge);
			Type vt = typeof(TVert);
			
			WeaverTitanUtil.GetAndVerifyElementAttribute<WeaverTitanEdgeAttribute> (et);
			WeaverTitanUtil.GetAndVerifyElementAttribute<WeaverTitanVertexAttribute> (vt);
			
			if ( pEdge.OutVertexType != vt && pEdge.InVertexType != vt ) {
				throw new WeaverException("Vertex type '"+vt.Name+"' is not valid for edge type '"+
					et.Name+"'.");
			}
			
			WeaverPropPair wpp = WeaverUtil.GetPropertyAttribute(pProperty);
			WeaverTitanPropertyAttribute att = WeaverTitanUtil.GetAndVerifyTitanPropertyAttribute(wpp);
			
			if ( !att.HasTitanVertexCentricIndex(et) ) {
				throw new WeaverException("Property '"+vt.Name+"."+wpp.Info.Name+"' does not have a "+
					"vertex-centric index for edge '"+et.Name+"'.");
			}
		}

	}

}