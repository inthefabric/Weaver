using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Core.Elements;

namespace Weaver.Core {

	/*================================================================================================*/
	public interface IWeaverConfig {

		IList<Type> VertexTypes { get; }
		IList<Type> EdgeTypes { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string GetEdgeDbName<T>(T pItem) where T : IWeaverEdge;

		/*--------------------------------------------------------------------------------------------*/
		string GetEdgeDbName<T>() where T : IWeaverEdge;

		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyDbName<T>(Expression<Func<T, object>> pExp) where T : IWeaverElement;

	}

}