import Controller = require("./Controller");
import ApiPromise = require("./ApiPromise");
import Foo = require("../Models/Foo");

class FooController extends Controller {

	public GetFoos(): ApiPromise<Array<Foo>> {
		return this.CallApi<Array<Foo>>({
			Verb: "GET",
			Endpoint: "Foo",
			Data: null			
		});
	}

	public GetFoo(fooId: number): ApiPromise<Foo> {
		return this.CallApi<Foo>({
			Verb: "GET",
			Endpoint: "Foo/" + fooId,
			Data: null			
		});
	}

	public CreateFoo(foo: Foo): ApiPromise<Foo> {
		return this.CallApi<Foo>({
			Verb: "POST",
			Endpoint: "Foo",
			Data: JSON.stringify(foo)			
		});
	}

	public DeleteFoo(fooId: number): ApiPromise<void> {
		return this.CallApi<void>({
			Verb: "DELETE",
			Endpoint: "Foo/" + fooId,
			Data: null			
		});
	}

	public UpdateFoo(foo: Foo): ApiPromise<Foo> {
		return this.CallApi<Foo>({
			Verb: "PATCH",
			Endpoint: "Foo",
			Data: JSON.stringify(foo)			
		});
	}
}

export = FooController;