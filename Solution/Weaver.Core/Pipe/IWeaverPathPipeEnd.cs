using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Core.Query;

namespace Weaver.Core.Pipe {
	
	/*================================================================================================*/
	public interface IWeaverPathPipeEnd : IWeaverPathItem{


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery ToQuery();

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery ToQueryAsVar(string pName, out IWeaverVarAlias pVar);

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery ToQueryAsVar<T>(string pName,out IWeaverVarAlias<T> pVar) where T : IWeaverElement;

	}

}