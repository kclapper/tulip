import { Connection } from "./Connection.js";
import { getEditor } from "./Editor.js";

const connection = await Connection.getInstance();
const editor = getEditor(connection);