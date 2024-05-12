import { MessageList } from "../MessageList.js";
import { MessageEditor } from "../Editor.js";

const ChatSessionWindowWidth = "500px";
const ChatSessionWindowHeight = "500px";

export class FloatingChatSession {
    #connection;

    #element;
    #chatSessionTab;
    #chatSessionWindow;

    #messageList;
    #editor;

    #windowIsVisible = false;

    constructor(connection, userName) {
        this.#connection = connection;

        this.#element = document.createElement("div");
        this.#element.className = "me-3 d-flex flex-column justify-content-end";

        /* Setup Chat Session Popup Window */
        this.#chatSessionWindow = makeMessageWindow(); 

        const messageListElement = makeMessageList();
        this.#messageList = new MessageList(connection, messageListElement);

        const spacer = document.createElement("div");
        spacer.className = "flex-grow-1";

        let [editorElement, messageInput, recipientInput] = makeEditor(userName); 
        this.#editor = new MessageEditor(connection, editorElement, messageInput, recipientInput);

        this.#chatSessionWindow.appendChild(messageListElement);
        this.#chatSessionWindow.appendChild(spacer);
        this.#chatSessionWindow.appendChild(editorElement);

        /* Chat session tab to toggle popup */
        this.#chatSessionTab = makeChatSessionTab(userName, this.close.bind(this));
        this.#chatSessionTab.onclick = this.toggleDisplay.bind(this);

        /* Add both popup and tab to element */
        this.#element.appendChild(this.#chatSessionWindow);
        this.#element.appendChild(this.#chatSessionTab);

        /* Add to floating chats */
        const floatingChatSessionList = document.getElementById("floating-chat-sessions");
        floatingChatSessionList.appendChild(this.#element);
    }

    toggleDisplay() {
        if (this.#windowIsVisible) {
            this.hide();
        } else {
            this.show();
        }
    }

    show() {
        const classToHide = " d-none"
        this.#chatSessionWindow.className = this.#chatSessionWindow.className.replace(classToHide, "");
        this.#windowIsVisible = true;
    }

    hide() {
        const classToHide = " d-none"
        this.#chatSessionWindow.className += classToHide;
        this.#windowIsVisible = false;
    }

    close() {
        this.#element.remove();
    }
}

function makeChatSessionTab(userName, closeHandler) {
    const button = document.createElement("button");
    button.className = "btn btn-outline-primary mt-auto p-2 d-flex align-items-center";

    const spacer = document.createElement("div");
    spacer.className = "flex-grow-1";
    button.appendChild(spacer);

    const name = document.createElement("div");
    name.innerText = userName;
    button.appendChild(name);
    button.appendChild(spacer.cloneNode());

    const closeButton = document.createElement("i"); 
    closeButton.className = "tf-icons bx bx-x ps-1";
    closeButton.style = "font-size: 1rem";
    closeButton.onclick = closeHandler;
    button.appendChild(closeButton);

    return button;
}

function makeMessageWindow() {
    const div = document.createElement("div");
    div.className = "d-flex flex-column bg-white border rounded mb-2 d-none";
    div.style = `width: ${ChatSessionWindowWidth}; height: ${ChatSessionWindowHeight}`;
    return div;
}

function makeMessageList() {
    const messageList = document.createElement("div");
    messageList.className = "container pt-4";
    return messageList;
}

function makeEditor(recipient) {
    const container = document.createElement("div");

    const row = document.createElement("div");
    row.className = "p-2 bg-white rounded";
    container.appendChild(row);

    const form = document.createElement("form");
    form.setAttribute("autocomplete", "off");
    row.appendChild(form);

    const recipientInput = document.createElement("input");
    recipientInput.setAttribute("type", "hidden");
    recipientInput.setAttribute("readonly", "true");
    recipientInput.setAttribute("value", recipient);
    form.appendChild(recipientInput);

    const messageInput = document.createElement("input");
    messageInput.setAttribute("type", "text");
    messageInput.setAttribute("spellcheck", "true");
    messageInput.className = "form-control";
    form.appendChild(messageInput);

    return [container, messageInput, recipientInput];
}
