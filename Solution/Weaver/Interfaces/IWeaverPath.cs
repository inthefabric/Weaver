using System.Collections.Generic;
using Weaver.Functions;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverPath {

		IWeaverQuery Query { get; }
		WeaverFuncIndex BaseIndex { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void AddItem(IWeaverItem pItem);
		string BuildParameterizedScript();

		/*--------------------------------------------------------------------------------------------*/
		int Length { get; }
		IWeaverItem ItemAtIndex(int pIndex);
		IList<IWeaverItem> PathToIndex(int pIndex, bool pInclusive=true);
		IList<IWeaverItem> PathFromIndex(int pIndex, bool pInclusive=true);

		/*--------------------------------------------------------------------------------------------*/
		int IndexOfItem(IWeaverItem pItem);
		TItem FindAsNode<TItem>(string pLabel) where TItem : IWeaverIndexableItem;

	}


	/*================================================================================================*/
	public interface IWeaverPath<out TBase> : IWeaverPath where TBase : class, IWeaverItem, new() {

		TBase BaseNode { get; }

	}

}