/* Connection events */
export const GetCurrentUser = "GetCurrentUser";
export const ReceiveMessage = "ReceiveMessage";
export const SendMessage = "SendMessage";
export const MessageSent = "MessageSent";

const connectionUrl = "/chatHub";

export class Connection {
    #currentUser;
    #connection;
    constructor() {
        if (!Connection.#isInternalInitialization) {
            throw new TypeError("Must use static factory method to create Connection");
        }

        this.#connection = new signalR.HubConnectionBuilder()
                                     .withUrl(connectionUrl)
                                     .withAutomaticReconnect()
                                     .build();

        
        this.#currentUser = this.#connection
                                .start()
                                .then(() => {
                                        return this.#connection.invoke(GetCurrentUser);
                                });
    }

    static #isInternalInitialization = false;
    static #instance = null;
    static async getInstance() {
        if (this.#instance == null) {
            this.#isInternalInitialization = true;
            this.#instance = new Connection();
            this.#isInternalInitialization = false;
        }

        this.#instance.#currentUser = await this.#instance.#currentUser;

        return this.#instance;
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