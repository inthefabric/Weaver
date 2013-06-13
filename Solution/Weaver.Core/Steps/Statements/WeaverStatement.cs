using Weaver.Core.Elements;

namespace Weaver.Core.Steps.Statements {

	/*================================================================================================*/
	public abstract class WeaverStatement<T> : IWeaverStatement<T> where T : IWeaverElement {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract string BuildParameterizedString();

	}

}