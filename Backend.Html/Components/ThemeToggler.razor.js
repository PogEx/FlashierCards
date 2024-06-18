    export class ThemeToggler {
         
    }

    export function setTheme(bTheme) {
        document.documentElement.setAttribute('data-bs-theme', bTheme ? "light" : "dark")
    }

window.ThemeToggler = ThemeToggler;
