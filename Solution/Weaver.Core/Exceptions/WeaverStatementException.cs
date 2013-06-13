using Weaver.Core.Elements;
using Weaver.Core.Steps.Statements;

namespace Weaver.Core.Exceptions {

	/*================================================================================================*/
	public class WeaverStatementException<T> : WeaverException where T : IWeaverElement {

		public IWeaverStatement<T> Statement { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStatementException(IWeaverStatement<T> pStatement, string pMessage) :
																	base("Statement", "", pMessage) {
			Statement = pStatement;
		}

	}

}