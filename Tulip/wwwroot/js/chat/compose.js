import { Connection } from "./Connection.js";
import { getEditor } from "./Editor.js";

const connection = new Connection();
const editor = getEditor(connection);