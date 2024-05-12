import { Connection } from "./components/connection/Connection.js";
import { MessageList } from "./components/MessageList.js";
import { MessageEditor } from "./components/Editor.js";

const connection = new Connection();

const messageListElement = document.getElementById("messageList");
const messageList = new MessageList(connection, messageListElement);

const messageBox = document.getElementById("messageInput");
const recipientBox = document.getElementById("recipientInput");
const editorElement = document.getElementById("messageEditor");
const editor = new MessageEditor(connection, editorElement, messageBox, recipientBox);