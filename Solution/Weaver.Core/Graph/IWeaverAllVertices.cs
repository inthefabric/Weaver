using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;

namespace Weaver.Core.Graph {

	/*================================================================================================*/
	public interface IWeaverAllVertices : IWeaverAllItems {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T ExactIndex<T>(Expression<Func<T, object>> pProperty, object pValue)
																		where T : IWeaverVertex, new();
		
	}

}