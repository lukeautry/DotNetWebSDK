import Controller = require("./Controller");
import ApiPromise = require("./ApiPromise");
import Widget = require("../Models/Widget");

class WidgetsController extends Controller {

	public GetWidgets(): ApiPromise<Array<Widget>> {
		return this.CallApi<Array<Widget>>({
			Verb: "GET",
			Endpoint: "Widgets",
			Data: null			
		});
	}

	public GetWidget(widgetId: number): ApiPromise<Widget> {
		return this.CallApi<Widget>({
			Verb: "GET",
			Endpoint: "Widgets/" + widgetId,
			Data: null			
		});
	}

	public CreateWidget(widget: Widget): ApiPromise<Widget> {
		return this.CallApi<Widget>({
			Verb: "POST",
			Endpoint: "Widgets",
			Data: JSON.stringify(widget)			
		});
	}

	public DeleteWidget(widgetId: number): ApiPromise<void> {
		return this.CallApi<void>({
			Verb: "DELETE",
			Endpoint: "Widgets/" + widgetId,
			Data: null			
		});
	}

	public UpdateWidget(widget: Widget): ApiPromise<Widget> {
		return this.CallApi<Widget>({
			Verb: "PATCH",
			Endpoint: "Widgets",
			Data: JSON.stringify(widget)			
		});
	}
}

export = WidgetsController;