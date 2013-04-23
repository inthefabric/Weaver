namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverNode : IWeaverItemWithId {

		bool IsFromNode { get; set; }
		bool ExpectOneNode { get; set; }
		bool IsRoot { get; }

	}

}