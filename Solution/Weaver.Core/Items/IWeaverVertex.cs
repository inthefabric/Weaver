namespace Weaver.Core.Items {

	/*================================================================================================*/
	public interface IWeaverVertex : IWeaverItemWithId {

		bool IsFromNode { get; set; }
		bool ExpectOneNode { get; set; }
		bool IsRoot { get; }

	}

}