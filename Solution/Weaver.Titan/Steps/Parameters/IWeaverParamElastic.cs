using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Steps.Parameters;

namespace Weaver.Titan.Steps.Parameters {

	/*================================================================================================*/
	public interface IWeaverParamElastic<T> : IWeaverParam<T> where T : IWeaverElement {

		Expression<Func<T, object>> Property { get; }
		WeaverParamElasticOp Operation { get; }
		object Value { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string GetOperationScript();

	}

}