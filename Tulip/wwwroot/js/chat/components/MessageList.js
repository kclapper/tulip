import { ReceiveMessage } from "./connection/Connection.js";

export class MessageList {
    #element;
    #connection;

    #lastMessageTimestamp;
    #lastMessageUser;
    #lastMessageTextElement;
    #lastMessageTimestampElement;

    constructor(connection, element) {
        // this.#element = document.getElementById("messageList");
        this.#element = element;
        if (!this.#element) {
            throw new Error("Element with id 'messageList' not found");
        }

        this.#connection = connection;
        this.#connection.on(ReceiveMessage, this.#addMessage.bind(this));

        this.resetScroll();
    }

    #addMessage(user, message) {
        if (this.#messageIsUpdate(user)) {
            this.#updateMessage(message);
            return;
        }

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

        const timestamp = new Date(); 
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

        this.#lastMessageTimestamp = timestamp;
        this.#lastMessageUser = user;
        this.#lastMessageTextElement = messageText; 
        this.#lastMessageTimestampElement = messageTimestamp;

        this.resetScroll();
    }

    #messageIsUpdate(user) {
        const isWithin10Seconds = new Date() - this.#lastMessageTimestamp <= 10000;
        const isSameUser = user == this.#lastMessageUser;
        return isWithin10Seconds && isSameUser;
    }

    #updateMessage(message) {
        this.#lastMessageTimestamp = new Date();
        this.#lastMessageTextElement.innerText += `\n\n${message}`;
        this.#lastMessageTimestampElement.textContent = `${this.#lastMessageTimestamp.toLocaleString()}`;

        this.resetScroll();
    }

    resetScroll() {
        if (this.#element.lastElementChild) {
            this.#element.lastElementChild.scrollIntoView();
        }
    }
}