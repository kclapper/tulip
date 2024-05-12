import { Connection } from "./components/connection/Connection.js";
import { FloatingChatSelector } from "./components/floatingChat/FloatingChatSelector.js";
import { FloatingChatSession } from "./components/floatingChat/FloatingChatSession.js";

const connection = new Connection();

const selector = new FloatingChatSelector(connection, document.getElementById("floating-chat-selector"));
const selectionButton = document.getElementById("floating-chat-selection-button");
selectionButton.onclick = selector.toggleDisplay.bind(selector);

const testSession = new FloatingChatSession(connection, "user");