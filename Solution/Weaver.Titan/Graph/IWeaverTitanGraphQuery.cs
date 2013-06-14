using Weaver.Core.Elements;
using Weaver.Titan.Steps.Parameters;

namespace Weaver.Titan.Graph {
	
	/*================================================================================================*/
	public interface IWeaverTitanGraphQuery {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T ElasticIndex<T>(params IWeaverParamElastic<T>[] pParams) where T : IWeaverElement, new();

	}

}