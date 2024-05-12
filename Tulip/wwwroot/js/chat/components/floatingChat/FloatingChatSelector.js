import { UserSearch } from "../UserSearch.js";
import { FloatingChatSession } from "./FloatingChatSession.js";

export class FloatingChatSelector {
    #connection;

    #element;
    #window;
    #form;
    #recipientInput;

    #windowIsVisible = false;

    #userSearch;

    constructor(connection, element) {
        this.#connection = connection;
        this.#element = element;

        this.#window = makeSelectorWindow();
        this.#element.appendChild(this.#window);

        let selectorElement, userSearchElement;
        [selectorElement, this.#form, this.#recipientInput, userSearchElement] = makeSelector();
        this.#window.appendChild(selectorElement);
        this.#form.onsubmit = this.handleSubmit.bind(this);

        this.#userSearch = new UserSearch(userSearchElement);
        this.#userSearch.onSelect((userName) => {
            this.#recipientInput.value = userName;
            this.handleSubmit();
        });
        this.#recipientInput.oninput = (event) => {
            this.#userSearch.query(this.#recipientInput.value);
        };
    }

    toggleDisplay() {
        if (this.#windowIsVisible) {
            this.hide();
        } else {
            this.show();
        }
    }

    show() {
        const classToHide = " d-none"
        this.#window.className = this.#window.className.replace(classToHide, "");
        this.#windowIsVisible = true;
    }

    hide() {
        const classToHide = " d-none"
        this.#window.className += classToHide;
        this.#windowIsVisible = false;
    }

    handleSubmit(event) {
        if (event) {
            event.preventDefault();
        }

        const newSession = new FloatingChatSession(this.#connection, this.#recipientInput.value);
        newSession.show();
        this.#recipientInput.value = "";
        this.hide();
    }
}

function makeSelectorWindow() {
    const div = document.createElement("div");
    div.className = "d-flex flex-column bg-white border rounded mb-2 d-none";
    div.style = `width: 250px;`;
    return div;
}

function makeSelector() {
    const container = document.createElement("div");

    const row = document.createElement("div");
    row.className = "p-2 bg-white rounded";
    container.appendChild(row);

    const form = document.createElement("form");
    form.setAttribute("autocomplete", "off");
    row.appendChild(form);

    const label = document.createElement("label");
    label.setAttribute("for", "floatingChatSelectorInput");
    label.innerText = "Select user"
    label.className = "pb-1"
    form.appendChild(label);

    const recipientInput = document.createElement("input");
    recipientInput.id = "floatingChatSelectorInput";
    recipientInput.setAttribute("type", "text");
    recipientInput.setAttribute("data-toggle", "dropdown");
    recipientInput.setAttribute("aria-haspopup", true);
    recipientInput.setAttribute("aria-expanded", false);
    recipientInput.setAttribute("required", true);
    recipientInput.className = "form-control dropdown-toggle";
    form.appendChild(recipientInput);

    const userSearch = document.createElement("div");
    userSearch.className = "dropdown-menu";
    userSearch.setAttribute("aria-labelledby", recipientInput.id);
    userSearch.setAttribute("hidden", true);
    form.appendChild(userSearch);
    
    return [container, form, recipientInput, userSearch];
}