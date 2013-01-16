using System.Collections.Generic;
using Weaver.Functions;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverPath {

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


	/*================================================================================================*/
	public interface IWeaverPath<out TBase> : IWeaverPath where TBase : class, IWeaverItem, new() {

		TBase BaseNode { get; }

	}


	/*================================================================================================*/
	public interface IWeaverPathFromVarAlias<TBase> : IWeaverPath<TBase>
													where TBase : class, IWeaverItemIndexable, new() {

		IWeaverVarAlias<TBase> BaseVar { get; }
		bool CopyItem { get; }

	}


	/*================================================================================================*/
	public interface IWeaverPathFromManualIndex<TBase> : IWeaverPath<TBase>
													where TBase : class, IWeaverItemIndexable, new() {

		WeaverFuncManualIndex<TBase> BaseIndex { get; }

	}


	/*================================================================================================*/
	public interface IWeaverPathFromKeyIndex<TBase> : IWeaverPath<TBase>
													where TBase : class, IWeaverItemIndexable, new() {

		WeaverFuncKeyIndex<TBase> BaseIndex { get; }

	}

}