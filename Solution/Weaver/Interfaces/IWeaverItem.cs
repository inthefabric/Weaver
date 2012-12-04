using System.Collections.Generic;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverItem {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverPath Path { get; set; }

		/*--------------------------------------------------------------------------------------------*/
		IWeaverItem PrevPathItem { get; }
		IWeaverItem NextPathItem { get; }
		IList<IWeaverItem> PathToThisItem { get; }
		IList<IWeaverItem> PathFromThisItem { get; }

		/*--------------------------------------------------------------------------------------------*/
		string ItemIdentifier { get; }

		/*--------------------------------------------------------------------------------------------*/
		string GremlinCode { get; }

	}

}