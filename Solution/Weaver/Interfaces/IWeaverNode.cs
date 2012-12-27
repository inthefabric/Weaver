namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverNode : IWeaverIndexableItem {

		long Id { get; set; }
		bool IsFromNode { get; set; }
		bool ExpectOneNode { get; set; }
		bool IsRoot { get; }

	}

}