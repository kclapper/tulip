import { MessageList } from "./MessageList.js";
import { getEditor } from "./Editor.js";
import { AiConnection } from "./AiConnection.js";

const connection = new AiConnection();

const messageList = new MessageList(connection);
const editor = getEditor(connection);
