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
		public static TEdge HasVci<TEdge, TVert>(this TEdge pEdge, 
						Expression<Func<TVert, object>> pProperty, WeaverStepHasOp pOperation,
						object pValue) where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState<TEdge, TVert>(pProperty);
			NewVert<TVert>(pEdge).Has(pProperty, pOperation, pValue);
			return pEdge;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TEdge HasVci<TEdge, TVert>(this TEdge pEdge, 
										Expression<Func<TVert, object>> pProperty) 
										where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState<TEdge, TVert>(pProperty);
			NewVert<TVert>(pEdge).Has(pProperty);
			return pEdge;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TEdge HasNotVci<TEdge, TVert>(this TEdge pEdge,
						Expression<Func<TVert, object>> pProperty, WeaverStepHasOp pOperation,
						object pValue) where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState<TEdge, TVert>(pProperty);
			NewVert<TVert>(pEdge).HasNot(pProperty, pOperation, pValue);
			return pEdge;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TEdge HasNotVci<TEdge, TVert>(this TEdge pEdge,
										Expression<Func<TVert, object>> pProperty) 
										where TEdge : IWeaverEdge where TVert : IWeaverVertex, new() {
			ConfirmVciState<TEdge, TVert>(pProperty);
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
		private static void ConfirmVciState<TEdge, TVert>(Expression<Func<TVert, object>> pProperty)
				                             	where TEdge : IWeaverEdge where TVert : IWeaverVertex {
			Type et = typeof(TEdge);
			Type vt = typeof(TVert);
			
			WeaverTitanUtil.GetAndVerifyElementAttribute<WeaverTitanEdgeAttribute>(et);
			WeaverTitanUtil.GetAndVerifyElementAttribute<WeaverTitanVertexAttribute>(vt);
			
			//TODO: vertify that TVert is valid for TEdge
			//TODO: or, create step that enters "vci" state for the specific vertex type, like:
			//	twoVert.InKnowsOne.StartVci.Has(x => x.A).ExitVci.ToVertex
			//where (StartVci implicity == StartVci<One>), and (ExitVci == ExitVci<InKnowsOne>)
			
			WeaverPropPair wpp = WeaverUtil.GetPropertyAttribute(pProperty);
			WeaverTitanPropertyAttribute att = WeaverTitanUtil.GetAndVerifyTitanPropertyAttribute(wpp);
			
			if ( !att.HasTitanVertexCentricIndex(et) ) {
				throw new WeaverException("Property '"+vt.Name+"."+wpp.Info.Name+"' does not have a "+
					"vertex-centric index for edge '"+et.Name+"'.");
			}
		}

	}

}