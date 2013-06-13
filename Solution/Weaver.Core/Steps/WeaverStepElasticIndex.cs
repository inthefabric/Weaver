using System.Text;
using Weaver.Core.Elements;
using Weaver.Core.Query;
using Weaver.Core.Steps.Parameters;

namespace Weaver.Core.Steps {
	
	/*================================================================================================*/
	//TEST: Core.Steps.WeaverStepElasticIndex
	public class WeaverStepElasticIndex<T> : WeaverStep where T : IWeaverElement {

		private readonly IWeaverParamElastic<T>[] vParams;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepElasticIndex(params IWeaverParamElastic<T>[] pParams) {
			vParams = pParams;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			var sb = new StringBuilder();

			foreach ( WeaverParamElastic<T> pe in vParams ) {
				string propName = WrapException(() => Path.Config.GetPropertyDbName(pe.Property));
				var valParam = Path.Query.AddParam(new WeaverQueryVal(pe.Value));
				
				sb.Append(
					(sb.Length > 0 ? "." : "")+
					"has('"+propName+"',"+pe.GetOperationScript()+","+valParam+")"
				);
			}

			return sb.ToString();
		}

	}

}