/* Connection events */
export const GetCurrentUser = "GetCurrentUser";
export const ReceiveMessage = "ReceiveMessage";
export const SendMessage = "SendMessage";
export const MessageSent = "MessageSent";


export class Connection {
    static connectionUrl = "/chatHub";

    #currentUser;
    #connection;
    constructor() {
        this.#connection = new signalR.HubConnectionBuilder()
                                     .withUrl(this.constructor.connectionUrl)
                                     .withAutomaticReconnect()
                                     .build();

        
        this.#connection
            .start()
            .then(() => {
                    return this.#connection.invoke(GetCurrentUser);
            }).then((currentUser) => this.#currentUser = currentUser);
    }

    get currentUser() {
        return this.#currentUser;
    }

    on(event, handler) {
        return this.#connection.on(event, handler);
    }

    off(event) {
        return this.#connection.off(event);
    }

    invoke(event, ...args) {
        return this.#connection.invoke(event, ...args);
    }
}