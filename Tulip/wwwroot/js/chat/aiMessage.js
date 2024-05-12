import { MessageList } from "./components/MessageList.js";
import { MessageEditor } from "./components/Editor.js";
import { AiConnection } from "./components/connection/AiConnection.js";
import { ReceiveMessage } from "./components/connection/Connection.js";

const typingIndicator = document.getElementById("typingIndicator");

const connection = new AiConnection();
connection.on(ReceiveMessage, (recipient, message) => {
    if (recipient == connection.currentUser) {
        editor.disable();
        typingIndicator.removeAttribute("hidden");        
    } else {
        editor.enable();
        typingIndicator.setAttribute("hidden", true);        
    }
});

const messageListElement = document.getElementById("messageList");
const messageList = new MessageList(connection, messageListElement);

const messageBox = document.getElementById("messageInput");
const recipientBox = document.getElementById("recipientInput");
const editorElement = document.getElementById("messageEditor");
const editor = new MessageEditor(connection, editorElement, messageBox, recipientBox);
