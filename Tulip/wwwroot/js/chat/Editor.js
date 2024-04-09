import { SendMessage, MessageSent } from "./Connection.js";
import { UserSearch } from "./userSearch.js";

class Editor {
    #element;
    constructor(connection, element) {
        this.connection = connection;

        this.#element = element;
        this.#element.addEventListener("submit", this.#submitHandler.bind(this));

        this.messageBox = document.getElementById("messageInput");
        this.recipientBox = document.getElementById("recipientInput");
    }

    #submitHandler(event) {
        event.preventDefault();
        this.sendMessage();
    }

    sendMessage() {
        const recipient = this.recipientBox.value;
        const message = this.messageBox.value;

        if (!message || !recipient) {
            return;
        }

        this.connection
            .invoke(SendMessage, recipient, message);

        this.messageBox.value = "";
    }
}

class MessageEditor extends Editor {
    constructor(connection) {
        super(connection, document.getElementById("messageEditor"));
    }
}

class ComposeEditor extends Editor {
    constructor(connection) {
        super(connection, document.getElementById("composeEditor"));

        this.userSearch = new UserSearch(document.getElementById("userSearch"));
        this.userSearch.onSelect((userName) => {
            this.recipientBox.value = userName;
        });
        this.recipientBox.oninput = (event) => {
            this.userSearch.query(this.recipientBox.value);
        };
    }

    sendMessage() {
        super.sendMessage();

        this.recipientBox.value = "";

        this.connection.on(MessageSent, (recipientId) => {
            window.location.href = `/Chat/Message/${recipientId}`;
        });
    }
}

export function getEditor(connection) {
    if (document.getElementById("messageEditor")) {
        return new MessageEditor(connection);
    }
    if (document.getElementById("composeEditor")) {
        return new ComposeEditor(connection);
    }
    throw new Error("No element with an editor id was found");
}