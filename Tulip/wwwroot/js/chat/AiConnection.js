import { Connection } from "./Connection.js";

/* Connection events */
export const GetCurrentUser = "GetCurrentUser";
export const ReceiveMessage = "ReceiveMessage";
export const SendMessage = "SendMessage";
export const MessageSent = "MessageSent";

export class AiConnection extends Connection {
    static connectionUrl = "/aiChatHub";

    constructor() {
        super();
    }
}