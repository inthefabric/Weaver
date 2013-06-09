using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;

namespace Weaver.Core.Graph {

	/*================================================================================================*/
	public interface IWeaverAllEdges : IWeaverAllItems {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T ExactIndex<T>(Expression<Func<T, object>> pProperty, object pValue) 
																		where T : IWeaverEdge, new();
		
	}

}