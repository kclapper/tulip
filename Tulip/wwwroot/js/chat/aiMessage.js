import { MessageList } from "./MessageList.js";
import { getEditor } from "./Editor.js";
import { AiConnection } from "./AiConnection.js";
import { ReceiveMessage } from "./Connection.js";

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

const messageList = new MessageList(connection);
const editor = getEditor(connection);
