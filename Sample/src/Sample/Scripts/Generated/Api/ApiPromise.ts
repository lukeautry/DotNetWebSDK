class ApiPromise<T> {
    private SuccessCallbacks = new Array<(value?: T) => any>();
    private ErrorCallbacks = new Array<(message?: string) => any>();

    public Success(callback: (value?: T) => any) {
        this.SuccessCallbacks.push(callback);
        return this;
    }

    public Error(callback: (message?: string) => any) {
        this.ErrorCallbacks.push(callback);
        return this;
    }

    public ResolveSuccess(value: any) {
        this.SuccessCallbacks.forEach(callback => callback(value));
    }

    public ResolveError(message: string) {
        this.ErrorCallbacks.forEach(callback => callback(message));
    }
}

export = ApiPromise;