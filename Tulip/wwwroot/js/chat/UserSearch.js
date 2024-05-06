export class UserSearch {
    #element;
    constructor(element) {
        this.#element = element;
    }

    async query(userString) {
        if (userString.length < 3) {
            this.#element.setAttribute("hidden", true);
            return;
        }
        this.#element.removeAttribute("hidden");

        const response = await fetch(`/Chat/UserSearch?query=${userString}`);
        const userResults = await response.json();

        const userElements = userResults.map(
            (userName) => this.#makeUserElement(userName)
        );

        this.#element.replaceChildren(...userElements);
    }

    #selectHandler;
    onSelect(handler) {
        this.#selectHandler = handler;
    }

    #makeUserElement(userName) {
        const userButton = document.createElement("button");
        userButton.className = "dropdown-item";
        userButton.type = "button";
        userButton.onclick = () => {
            if (this.#selectHandler) {
                this.#selectHandler(userName)
            }
        };
        userButton.innerText = userName;
        return userButton;
    }
}