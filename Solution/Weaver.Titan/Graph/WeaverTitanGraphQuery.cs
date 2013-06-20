using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Titan.Steps;
using Weaver.Titan.Steps.Parameters;

namespace Weaver.Titan.Graph {

	/*================================================================================================*/
	public class WeaverTitanGraphQuery : WeaverPathItem, IWeaverTitanGraphQuery {

		private readonly bool vVertMode;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverTitanGraphQuery(bool pVertMode) {
			vVertMode = pVertMode;
		}

		/*--------------------------------------------------------------------------------------------*/
		public T ElasticIndex<T>(params IWeaverParamElastic<T>[] pParams)
																	where T : IWeaverElement, new() {
			var ei = new WeaverStepElasticIndex<T>(vVertMode, pParams);
			Path.AddItem(ei);
			return new T { Path = Path };
		}

		/*--------------------------------------------------------------------------------------------*/
		public T ElasticIndex<T>(Expression<Func<T, object>> pProperty, string pSpaceDelimitedText)
																	where T : IWeaverElement, new() {
			var ei = new WeaverStepElasticIndex<T>(vVertMode, pProperty, pSpaceDelimitedText);
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