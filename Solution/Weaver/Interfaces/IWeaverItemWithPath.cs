namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverItemWithPath {

		IWeaverPath Path { get; set; }
		int PathIndex { get; }

	}

}