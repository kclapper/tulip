import { Connection } from "./components/connection/Connection.js";
import { FloatingChatSelector } from "./components/floatingChat/FloatingChatSelector.js";

const connection = new Connection();

const selector = new FloatingChatSelector(connection, document.getElementById("floating-chat-selector"));
const selectionButton = document.getElementById("floating-chat-selection-button");
selectionButton.onclick = selector.toggleDisplay.bind(selector);