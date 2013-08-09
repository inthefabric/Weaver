using Weaver.Core.Elements;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public interface IWeaverStepAsColumn : IWeaverStep {

		string PropName { get; }
		string AppendScript { get; set; }
		string ReplaceScript { get; set; }

	}
	
	/*================================================================================================*/
	public interface IWeaverStepAsColumn<out TItem> : IWeaverStepAsColumn, IWeaverStepAs<TItem>
																		where TItem : IWeaverElement {

	}

}