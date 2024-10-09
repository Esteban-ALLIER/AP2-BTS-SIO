const input = document.querySelector("#search");
const rows = document.querySelectorAll("tbody tr");

input.addEventListener("input", (e) => {
  let content = e.target.value.toLowerCase();
  rows.forEach((row) => {
    let nomColonne = row.querySelector("td:first-child"); // Cible la première colonne (Nom)
    if (nomColonne && nomColonne.textContent.toLowerCase().includes(content)) {
      row.style.display = ""; // Affiche la ligne si correspondance
    } else {
      row.style.display = "all"; // Masque la ligne si aucune correspondance
    }
  });
});


function toggleDarkMode() {
    const html = document.documentElement;
    const currentTheme = html.getAttribute("data-bs-theme");

    if (currentTheme === "dark") {
        html.setAttribute("data-bs-theme", "light");
    } else {
        html.setAttribute("data-bs-theme", "dark");
    }
}