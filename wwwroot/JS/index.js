document.addEventListener("DOMContentLoaded", () => {
    const input = document.querySelector("#search");
    const rows = document.querySelectorAll("tbody tr");

    input.addEventListener("input", (e) => {
        let content = e.target.value.toLowerCase();

        rows.forEach((row) => {
            let nomColonne = row.querySelector("td:first-child"); // Cible la première colonne
            let prenomColonne = row.querySelector("td:nth-child(2)");
            let sexeColonne = row.querySelector("td:nth-child(3)");
            let numSecuColonne = row.querySelector("td:nth-child(4)");

            // Vérifie si l'une des colonnes contient le texte recherché
            if (
                (nomColonne && nomColonne.textContent.toLowerCase().includes(content)) ||
                (prenomColonne && prenomColonne.textContent.toLowerCase().includes(content)) ||
                (sexeColonne && sexeColonne.textContent.toLowerCase().includes(content)) ||
                (numSecuColonne && numSecuColonne.textContent.toLowerCase().includes(content))
            ) {
                row.style.display = ""; // Affiche la ligne si correspondance
            } else {
                row.style.display = "none"; // Masque la ligne si aucune correspondance
            }
        });
    });
});

const body = document.body;
const navbar = document.querySelector('.navbar');
const footer = document.querySelector('footer');
const darkModeToggle = document.getElementById('darkModeToggle');

if (localStorage.getItem('dark-mode') === 'enabled') {
    enableDarkMode();
    darkModeToggle.checked = true;
}

function enableDarkMode() {
    body.classList.add('dark-mode');
    navbar.classList.add('dark-mode');
    footer.classList.add('dark-mode');
    body.classList.remove('light-mode');
    navbar.classList.remove('light-mode');
    footer.classList.remove('light-mode');
    localStorage.setItem('dark-mode', 'enabled');
}

function disableDarkMode() {
    body.classList.add('light-mode');
    navbar.classList.add('light-mode');
    footer.classList.add('light-mode');
    body.classList.remove('dark-mode');
    navbar.classList.remove('dark-mode');
    footer.classList.remove('dark-mode');
    localStorage.setItem('dark-mode', 'disabled');
}

darkModeToggle.addEventListener('change', function () {
    if (this.checked) {
        enableDarkMode();
    } else {
        disableDarkMode();
    }
});

