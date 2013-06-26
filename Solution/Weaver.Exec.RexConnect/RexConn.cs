namespace Weaver.Exec.RexConnect {

	/*================================================================================================*/
	public static class RexConn {

		public enum Command {
			Session = 1,
			Query,
			Config
		}

		public enum SessionAction {
			Start = 1,
			Close,
			Commit,
			Rollback
		}

		public enum ConfigSetting {
			Debug = 1,
			Pretty
		}

		public enum GraphElementType {
			Unknown,
			Vertex,
			Edge,
			Error
		}

	}

}