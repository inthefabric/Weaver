using System.Collections.Generic;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverPath Path { get; set; }

		/*--------------------------------------------------------------------------------------------*/
		IWeaverItem PrevQueryItem { get; }
		IWeaverItem NextQueryItem { get; }
		IList<IWeaverItem> QueryPathToThisItem { get; }
		IList<IWeaverItem> QueryPathFromThisItem { get; }

		/*--------------------------------------------------------------------------------------------*/
		string ItemIdentifier { get; }

		/*--------------------------------------------------------------------------------------------*/
		string GremlinCode { get; }

	}

}