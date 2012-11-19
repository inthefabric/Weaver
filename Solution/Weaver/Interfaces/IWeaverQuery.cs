using System.Collections.Generic;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverQuery {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void AddQueryItem(IWeaverItem pItem);

		/*--------------------------------------------------------------------------------------------*/
		int PathLength();
		IWeaverItem PathAtIndex(int pIndex);
		IList<IWeaverItem> PathToIndex(int pIndex, bool pInclusive=true);
		IList<IWeaverItem> PathFromIndex(int pIndex, bool pInclusive=true);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		TItem FindAsNode<TItem>(string pLabel) where TItem : IWeaverQueryItem;
		
		/*--------------------------------------------------------------------------------------------*/
		string GremlinCode { get; }

	}

}