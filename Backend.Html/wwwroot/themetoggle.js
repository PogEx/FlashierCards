(() => {
    'use strict'

    const getStoredTheme = () => localStorage.getItem('theme')
    const setStoredTheme = theme => localStorage.setItem('theme', theme)

    const getPreferredTheme = () => {
        const storedTheme = getStoredTheme()
        if (storedTheme) {
            return storedTheme
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
    }

    const setTheme = theme => {
        document.documentElement.setAttribute('data-bs-theme', theme)
    }

    setTheme(getPreferredTheme())

    const showActiveTheme = (theme, focus = false) => {
        const themeSwitcher = document.querySelector('#bd-theme')

        if (!themeSwitcher) {
            return
        }
        
        themeSwitcher.checked = theme === 'light';
        
        themeSwitcher.addEventListener('change',  (event) => {
            if(event.target.checked){
                setTheme('light')
                setStoredTheme('light')
            }else{
                setTheme('dark')
                setStoredTheme('dark')
            }
        })
        
        if (focus) {
            themeSwitcher.focus()
        }
    }
    
    window.addEventListener('DOMContentLoaded', () => {
        showActiveTheme(getPreferredTheme())

        const themeSwitcher = document.querySelector('#bd-theme')

        if (!themeSwitcher) {
            return
        }
        const theme = themeSwitcher.checked ? 'light' : 'dark';
        setTheme(theme)
        setStoredTheme(theme)
        showActiveTheme(theme, true)
    })
})()