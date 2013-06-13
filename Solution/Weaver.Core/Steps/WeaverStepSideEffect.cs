using System.Text;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Steps.Statements;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepSideEffect<T> : WeaverStep where T : IWeaverElement {

		private readonly IWeaverStatement<T>[] vStatement;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepSideEffect(params IWeaverStatement<T>[] pStatements) {
			vStatement = pStatements;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			if ( vStatement.Length == 0 ) {
				throw new WeaverStepException(this, "SideEffect must have at least one Statement.");
			}

			var sb = new StringBuilder("sideEffect{");

			foreach ( IWeaverStatement<T> s in vStatement ) {
				sb.Append(s.BuildParameterizedString()+";");
			}

			sb.Append("}");
			return sb.ToString();
		}

	}

}