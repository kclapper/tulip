import { SendMessage, MessageSent } from "./connection/Connection.js";
import { UserSearch } from "./UserSearch.js";

class Editor {
    #element;
    constructor(connection, element, messageBox, recipientBox) {
        this.connection = connection;

        this.#element = element;
        this.#element.addEventListener("submit", this.#submitHandler.bind(this));

        this.messageBox = messageBox;
        this.recipientBox = recipientBox;
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

    disable() {
        this.messageBox.setAttribute("disabled", true);
    }

    enable() {
        this.messageBox.removeAttribute("disabled");
    }
}

export class MessageEditor extends Editor {
    constructor(connection, element, messageBox, recipientBox) {
        super(connection, element, messageBox, recipientBox);
    }
}

export class ComposeEditor extends Editor {
    constructor(connection, element, userSearch, messageBox, recipientBox) {
        super(connection, element, messageBox, recipientBox);

        this.userSearch = new UserSearch(userSearch);
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
