using Weaver.Core.Elements;
using Weaver.Core.Path;

namespace Weaver.Core.Steps.Statements {

	/*================================================================================================*/
	public interface IWeaverStatement<T> where T : IWeaverElement {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string BuildParameterizedString(IWeaverPath pPath);

	}

}