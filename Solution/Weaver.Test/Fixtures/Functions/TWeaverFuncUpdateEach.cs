using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncUpdateEach : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var person = new Person();
			var updates = new WeaverUpdates<Person>();
			updates.AddUpdate(person, p => p.PersonId);

			var f = new WeaverFuncUpdateEach<Person>(updates);
			Assert.NotNull(f, "Incorrect new().");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewEmptyFail() {
			var updates = new WeaverUpdates<Person>();

			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, () => {
				var f = new WeaverFuncUpdateEach<Person>(updates);
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString1() {
			var person = new Person();
			var updates = new WeaverUpdates<Person>();
			updates.AddUpdate(person, p => p.PersonId);

			var mockQuery = new Mock<IWeaverQuery>();
			mockQuery.Setup(x => x.AddParam(updates[0].Value)).Returns("321");

			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);

			var f = new WeaverFuncUpdateEach<Person>(updates);
			f.Path = mockPath.Object;

			Assert.AreEqual("sideEffect{it.setProperty('PersonId',321)}", f.BuildParameterizedString(),
				"Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString4() {
			var person = new Person();
			var updates = new WeaverUpdates<Person>();
			updates.AddUpdate(person, p => p.PersonId);
			updates.AddUpdate(person, p => p.Name);
			updates.AddUpdate(person, p => p.IsMale);
			updates.AddUpdate(person, p => p.Age);
			
			var mockQuery = new Mock<IWeaverQuery>();
			mockQuery.Setup(x => x.AddParam(updates[0].Value)).Returns("321");
			mockQuery.Setup(x => x.AddParam(updates[1].Value)).Returns("_P0");
			mockQuery.Setup(x => x.AddParam(updates[2].Value)).Returns("true");
			mockQuery.Setup(x => x.AddParam(updates[3].Value)).Returns("27.3");

			var mockPath = new Mock<IWeaverPath>();
			mockPath.SetupGet(x => x.Query).Returns(mockQuery.Object);

			var f = new WeaverFuncUpdateEach<Person>(updates);
			f.Path = mockPath.Object;

			Assert.AreEqual("sideEffect{it.setProperty('PersonId',321);it.setProperty('Name',_P0);"+
				"it.setProperty('IsMale',true);it.setProperty('Age',27.3)}",
				f.BuildParameterizedString(), "Incorrect result.");
		}

	}

}