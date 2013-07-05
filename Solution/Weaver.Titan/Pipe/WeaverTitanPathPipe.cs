using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Core.Steps.Statements;
using Weaver.Core.Util;
using Weaver.Titan.Elements;
using Weaver.Core.Exceptions;

namespace Weaver.Core.Pipe {
	
	/*================================================================================================*/
	public static class WeaverTitanPathPipeExt {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static TVert HasVci<TEdge, TVert>(this TEdge pEdge, 
							Expression<Func<TVert, object>> pProperty, WeaverStepHasOp pOperation,
							object pValue) where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState<TEdge, TVert>(pProperty);
			return (new TVert()).Has(pProperty, pOperation, pValue);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TVert HasVci<TEdge, TVert>(this TEdge pEdge, 
										Expression<Func<TVert, object>> pProperty) 
										where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState<TEdge, TVert>(pProperty);
			return (new TVert()).Has(pProperty);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TVert HasNotVci<TEdge, TVert>(this TEdge pEdge,
						Expression<Func<TVert, object>> pProperty, WeaverStepHasOp pOperation,
						object pValue) where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState<TEdge, TVert>(pProperty);
			return (new TVert()).HasNot(pProperty, pOperation, pValue);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TVert HasNotVci<TEdge, TVert>(this TEdge pEdge,
										Expression<Func<TVert, object>> pProperty) 
										where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState<TEdge, TVert>(pProperty);
			return (new TVert()).HasNot(pProperty);
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static void ConfirmVciState<TEdge, TVert>(Expression<Func<TVert, object>> pProperty)
				                             	where TEdge : IWeaverEdge where TVert : IWeaverVertex {
			Type et = typeof(TEdge);
			Type vt = typeof(TVert);
			
			WeaverTitanUtil.GetAndVerifyElementAttribute<WeaverTitanEdgeAttribute>(et);
			WeaverTitanUtil.GetAndVerifyElementAttribute<WeaverTitanVertexAttribute>(vt);
			
			WeaverPropPair wpp = WeaverUtil.GetPropertyAttribute(pProperty);
			WeaverTitanPropertyAttribute att = WeaverTitanUtil.GetAndVerifyTitanPropertyAttribute(wpp);
			
			if ( !att.HasTitanVertexCentricIndex(et) ) {
				throw new WeaverException("Property '"+wpp.Info.Name+"' does not have a vertex-"+
					"centric index for edge '"+et.Name+"' and vertex '"+vt.Name+"'.");
			}
		}

	}

}