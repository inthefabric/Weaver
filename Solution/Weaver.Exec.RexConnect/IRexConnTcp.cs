using System.Net.Sockets;

namespace Weaver.Exec.RexConnect {
	
	/*================================================================================================*/
	public interface IRexConnTcp {

		int SendBufferSize { get; set; }
		int ReceiveBufferSize { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		NetworkStream GetStream();

	}

}