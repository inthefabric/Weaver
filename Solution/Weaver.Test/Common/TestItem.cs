using Moq;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Test.Common {

	/*================================================================================================*/
	public class TestItem : WeaverItem, IWeaverItemIndexable {

		public Mock<IWeaverPath> MockPath { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestItem() {
			MockPath = new Mock<IWeaverPath>();
			Path = MockPath.Object;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return null;
		}

	}

}