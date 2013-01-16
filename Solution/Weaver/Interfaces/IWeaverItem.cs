using System.Collections.Generic;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverItem : IWeaverItemWithPath {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverItem PrevPathItem { get; }
		IWeaverItem NextPathItem { get; }
		IList<IWeaverItem> PathToThisItem { get; }
		IList<IWeaverItem> PathFromThisItem { get; }

		/*--------------------------------------------------------------------------------------------*/
		string ItemIdentifier { get; }
		bool SkipDotPrefix { get; }

		/*--------------------------------------------------------------------------------------------*/
		string BuildParameterizedString();

	}

}