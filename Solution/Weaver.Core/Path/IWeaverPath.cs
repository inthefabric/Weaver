using System.Collections.Generic;
using Weaver.Core.Query;

namespace Weaver.Core.Path {

	/*================================================================================================*/
	public interface IWeaverPath {

		IWeaverConfig Config { get; }
		IWeaverQuery Query { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void AddItem(IWeaverPathItem pItem);
		string BuildParameterizedScript();

		/*--------------------------------------------------------------------------------------------*/
		int Length { get; }
		IWeaverPathItem ItemAtIndex(int pIndex);
		IList<IWeaverPathItem> PathToIndex(int pIndex, bool pInclusive=true);
		IList<IWeaverPathItem> PathFromIndex(int pIndex, bool pInclusive=true);

		/*--------------------------------------------------------------------------------------------*/
		int IndexOfItem(IWeaverPathItem pItem);

	}

}