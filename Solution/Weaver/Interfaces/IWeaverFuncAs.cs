using System;
using Weaver.Functions;
using Weaver.Items;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverFuncAs<TItem> : IWeaverFunc where TItem : IWeaverItemIndexable {

		TItem Item { get; }
		string Label { get; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string BuildParameterizedString();

	}

}