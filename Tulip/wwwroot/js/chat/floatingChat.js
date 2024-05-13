import { Connection } from "./components/connection/Connection.js";
import { FloatingChatSelector } from "./components/floatingChat/FloatingChatSelector.js";
import { FloatingChatSession } from "./components/floatingChat/FloatingChatSession.js";

const connection = new Connection();

const selector = new FloatingChatSelector(connection, document.getElementById("floating-chat-selector"));
const selectionButton = document.getElementById("floating-chat-selection-button");
selectionButton.onclick = selector.toggleDisplay.bind(selector);

connection.onStarted(() => {
    fetch(`/Chat/GetFloatingChats`)
        .then((response) => {
            if (response.status != 200) {
                return Promise.resolve([]);
            }

            return response.json();
        })
        .then((result) => {
            for (const chat of result) {
                const floatingChat = new FloatingChatSession(connection, chat["otherUserName"]);
                if (chat["isActive"]) {
                    floatingChat.show();
                } else {
                    floatingChat.hide();
                }
            }
        });
});