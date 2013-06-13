using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Schema;

namespace Weaver.Core {

	/*================================================================================================*/
	public interface IWeaverConfig {

		IList<WeaverVertexSchema> VertexSchemas { get; }
		IList<WeaverEdgeSchema> EdgeSchemas { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string GetItemDbName<T>(T pItem) where T : IWeaverElement;
		string GetItemDbName<T>() where T : IWeaverElement;
		string GetItemDbName(string pItemName);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyDbName<T>(Expression<Func<T, object>> pExp) where T : IWeaverElement;
		
		/*--------------------------------------------------------------------------------------------*/
		string GetPropertyDbName<T>(string pProp) where T : IWeaverElement;

	}

}