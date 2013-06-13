using Weaver.Core.Elements;

namespace Weaver.Core.Steps.Statements {

	/*================================================================================================*/
	public interface IWeaverStatement<T> where T : IWeaverElement {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string BuildParameterizedString();

	}

}