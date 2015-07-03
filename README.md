# Overview

Keeping your .NET classes in sync with your client-side (web-based) code is annoying and a breeding ground for bugs. What if you could have a single source of truth in your C# code?

The concept is already well known with ORMs, notably Entity Framework. Your build your POCOs, register them properly, and the database schema with the correct relationships is automatically built out by EF. You've probably heard the term "code first migrations" in EF.

The goal of this project is to implement something similar to code first migrations, except instead of .NET models => Database Schema, it's .NET models to:

-TypeScript classes

public class Foo {
  public string Id { get; set; }
}

becomes...

class Foo {
  Id: string;
}

-Optionally, an API wrapper for use with ASP.NET Web API

[Route("Foos")]
[AcceptVerbs("POST")]
public async Task<Foo> Create([FromBody]Foo foo)
{
    return await new FooRepository().Create(foo);
}

becomes...

class FooQuery {
  public static Create(Foo foo): Promise<Foo> {
    // some ajax stuff
    return createdFoo;
  }
}
