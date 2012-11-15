namespace Fabric.Domain.Graph.Interfaces {

	/*================================================================================================*/
	public interface IWeaverProp {

		TToItem Back<TToItem>(string pLabel) where TToItem : IWeaverItem;

	}

}