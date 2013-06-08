using System.Collections.Generic;
using Weaver.Core.Items;
using Weaver.Core.Query;

namespace Weaver.Core.Path {

	/*================================================================================================*/
	public interface IWeaverPath {

		IWeaverConfig Config { get; }
		IWeaverQuery Query { get; }


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

	}

}