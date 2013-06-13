using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;

namespace Weaver.Core.Graph {

	/*================================================================================================*/
	public class WeaverAllVertices : WeaverAllItems, IWeaverAllVertices {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T Id<T>(string pId) where T : IWeaverVertex, new() {
			return IdInner<T>(pId);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public T ExactIndex<T>(Expression<Func<T, object>> pProperty, object pValue)
																		where T : IWeaverVertex, new() {
			return ExactIndexInner(pProperty, pValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return (ForSpecificId ? "v" : "V");
		}

	}

}