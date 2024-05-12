import { Connection } from "./components/connection/Connection.js";
import { ComposeEditor } from "./components/Editor.js";

const connection = new Connection();

const messageBox = document.getElementById("messageInput");
const recipientBox = document.getElementById("recipientInput");
const userSearch = document.getElementById("userSearch");
const editorElement = document.getElementById("composeEditor");
const editor = new ComposeEditor(connection, editorElement, userSearch, messageBox, recipientBox);