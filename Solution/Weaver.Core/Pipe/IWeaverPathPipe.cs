﻿
namespace Weaver.Core.Pipe {
	
	/*================================================================================================*/
	public interface IWeaverPathPipe : IWeaverPathPipeEnd {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd Count();

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd Iterate();

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd Remove();

	}

}