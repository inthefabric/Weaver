## Weaver

Weaver provides a fluent, strongly-typed interface for generating Gremlin scripts (for .NET/C#).

It requires a full graph schema (with [nodes](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver/Schema/WeaverNodeSchema.cs) and [relationships](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver/Schema/WeaverRelSchema.cs)) to function correctly.

#### Basic Usage

Weaver converts C# code:
```cs
myWeaverObj.BeginPath<User>(x => x.Name, "Zach")
```

...into Gremlin script:
```
g.V('U_Na','Zach')
```

The Gremlin script can also be parameterized (enabled by default) to achieve more efficient query compilation on the database side.

#### Fabric

Weaver was built to support the [Fabric](https://github.com/inthefabric/Fabric) project, which provides several [useful examples](https://github.com/inthefabric/Fabric/blob/master/Solution/Fabric.Api.Modify/Tasks/ModifyTasks.cs) of Weaver configuration, setup, and usage. 

A slightly-modified example from Fabric [code](https://github.com/inthefabric/Fabric/blob/master/Solution/Fabric.Api.Modify/Tasks/ModifyTasks.cs#L50):
```cs
IWeaverFuncAs<Member> memAlias;

IWeaverQuery q = 
	myWeaverObj.BeginPath<User>(x => x.ArtifactId, 123).BaseNode
	.DefinesMemberList.ToMember
		.As(out memAlias)
	.InAppDefines.FromApp
		.Has(x => x.ArtifactId, WeaverFuncHasOp.EqualTo, 456)
	.Back(memAlias)
	.HasMemberTypeAssign.ToMemberTypeAssign
		.Has(x => x.MemberTypeId, WeaverFuncHasOp.NotEqualTo, (byte)MemberTypeId.None)
		.Has(x => x.MemberTypeId, WeaverFuncHasOp.NotEqualTo, (byte)MemberTypeId.Invite)
		.Has(x => x.MemberTypeId, WeaverFuncHasOp.NotEqualTo, (byte)MemberTypeId.Request)
	.Back(memAlias)
	.End();

SendGremlinRequest(q.Script, q.Params);
```


[![githalytics.com alpha](https://cruel-carlota.pagodabox.com/9caca4070a7a2601105b67a6840644c2 "githalytics.com")](http://githalytics.com/inthefabric/Weaver)
