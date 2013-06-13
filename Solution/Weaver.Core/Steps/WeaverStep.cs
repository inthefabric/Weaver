using System;
using Weaver.Core.Exceptions;
using Weaver.Core.Pipe;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public abstract class WeaverStep : WeaverPathPipe, IWeaverStep {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected TResult WrapException<TResult>(Func<TResult> pFunc) {
			try {
				return pFunc();
			}
			catch ( WeaverException e ) {
				throw new WeaverStepException(this, e.Message);
			}
		}

	}

}