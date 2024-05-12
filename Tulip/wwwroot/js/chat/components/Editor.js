import { SendMessage, MessageSent } from "./connection/Connection.js";
import { UserSearch } from "./UserSearch.js";

class Editor {
    #element;
    constructor(connection, element, messageBox, recipientBox) {
        this.connection = connection;

        this.#element = element;
        this.#element.addEventListener("submit", this.#submitHandler.bind(this));

        // this.messageBox = document.getElementById("messageInput");
        // this.recipientBox = document.getElementById("recipientInput");
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
        // super(connection, document.getElementById("messageEditor"));
        super(connection, element, messageBox, recipientBox);
    }
}

export class ComposeEditor extends Editor {
    constructor(connection, element, userSearch, messageBox, recipientBox) {
        // super(connection, document.getElementById("composeEditor"));
        super(connection, element, messageBox, recipientBox);

        // this.userSearch = new UserSearch(document.getElementById("userSearch"));
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

// export function getEditor(connection) {
//     const messageBox = document.getEle

//     if (document.getElementById("messageEditor")) {
//         return new MessageEditor(connection, document.getElementById("messageEditor"));
//     }

//     if (document.getElementById("composeEditor")) {
//         return new ComposeEditor(connection, document.getElementById("composeEditor"));
//     }
//     throw new Error("No element with an editor id was found");
// }