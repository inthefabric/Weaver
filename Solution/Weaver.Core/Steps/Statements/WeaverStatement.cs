﻿using System;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;

namespace Weaver.Core.Steps.Statements {

	/*================================================================================================*/
	public abstract class WeaverStatement<T> : IWeaverStatement<T> where T : IWeaverElement {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract string BuildParameterizedString(IWeaverPath pPath);

		/*--------------------------------------------------------------------------------------------*/
		protected TResult WrapException<TResult>(Func<TResult> pFunc) {
			try {
				return pFunc();
			}
			catch ( WeaverException e ) {
				throw new WeaverStatementException<T>(this, e.Message);
			}
		}

	}

}