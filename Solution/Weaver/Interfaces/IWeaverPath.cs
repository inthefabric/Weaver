using System.Collections.Generic;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverPath {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void AddItem(IWeaverItem pItem);

		/*--------------------------------------------------------------------------------------------*/
		int Length { get; }
		IWeaverItem ItemAtIndex(int pIndex);
		IList<IWeaverItem> PathToIndex(int pIndex, bool pInclusive=true);
		IList<IWeaverItem> PathFromIndex(int pIndex, bool pInclusive=true);

		/*--------------------------------------------------------------------------------------------*/
		int IndexOfItem(IWeaverItem pItem);
		TItem FindAsNode<TItem>(string pLabel) where TItem : IWeaverItem;
		
		/*--------------------------------------------------------------------------------------------*/
		string GremlinCode { get; }

	}

}