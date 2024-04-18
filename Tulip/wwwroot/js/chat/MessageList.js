import { ReceiveMessage } from "./Connection.js";

export class MessageList {
    #element;
    #connection;
    constructor(connection) {
        this.#element = document.getElementById("messageList");
        if (!this.#element) {
            throw new Error("Element with id 'messageList' not found");
        }

        this.#connection = connection;
        this.#connection.on(ReceiveMessage, this.#addMessage.bind(this));

        this.resetScroll();
    }

    #addMessage(user, message) {
        const isUserMessage = user === this.#connection.currentUser;

        const newMessage = document.createElement("div");
        newMessage.className = "row no-gutters";

        const spacer = document.createElement("div");
        spacer.className = "col-7";

        const messageRow = document.createElement("div");
        messageRow.className = "col-5";

        const messageCard = document.createElement("div");
        messageCard.className = "card";

        const messageText = document.createElement("div");
        messageText.className = "card-body p-3";
        messageText.textContent = message;

        const messageTimestamp = document.createElement("p")
        messageTimestamp.className = isUserMessage ? "text-muted text-end" : "text-muted text-start";
        messageTimestamp.textContent = `${(new Date()).toLocaleString()}`;

        messageCard.appendChild(messageText);
        messageRow.appendChild(messageCard);
        messageRow.appendChild(messageTimestamp);

        if (isUserMessage) {
            newMessage.appendChild(spacer);
            newMessage.appendChild(messageRow)
        } else {
            newMessage.appendChild(messageRow)
            newMessage.appendChild(spacer);
        }

        this.#element.appendChild(newMessage);

        this.resetScroll();
    }

    resetScroll() {
        if (this.#element.lastElementChild) {
            this.#element.lastElementChild.scrollIntoView();
        }
    }
}