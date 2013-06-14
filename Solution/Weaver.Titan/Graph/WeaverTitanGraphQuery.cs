using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Titan.Steps;
using Weaver.Titan.Steps.Parameters;

namespace Weaver.Titan.Graph {

	/*================================================================================================*/
	public class WeaverTitanGraphQuery : WeaverPathItem, IWeaverTitanGraphQuery {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T ElasticIndex<T>(params IWeaverParamElastic<T>[] pParams)
																	where T : IWeaverElement, new() {
			var ei = new WeaverStepElasticIndex<T>(pParams);
			Path.AddItem(ei);
			return new T { Path = Path };
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return "query()";
		}

	}

}