import { MessageList } from "./MessageList.js";
import { getEditor } from "./Editor.js";
import { AiConnection } from "./AiConnection.js";
import { ReceiveMessage } from "./Connection.js";

const connection = new AiConnection();
connection.on(ReceiveMessage, (recipient, message) => {
    if (recipient == connection.currentUser) {
        editor.disable();
    } else {
        editor.enable();
    }
});

const messageList = new MessageList(connection);
const editor = getEditor(connection);
