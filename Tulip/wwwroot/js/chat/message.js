import { Connection } from "./Connection.js";
import { MessageList } from "./MessageList.js";
import { getEditor } from "./Editor.js";

const connection = new Connection();

const messageList = new MessageList(connection);
const editor = getEditor(connection);