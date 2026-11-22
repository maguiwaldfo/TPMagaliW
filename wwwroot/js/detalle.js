
document.getElementById("fechaOcupacion").addEventListener("change", function () {

    let fecha = document.getElementById("fechaOcupacion").value;

    let idEspacio = document.getElementById("IdEspacio").value;

    fetch("/Espacios/OcupacionDelDia?idEspacio=" + idEspacio + "&fecha=" + fecha)
        .then(response => response.json())
        .then(reservas => {

            let contenido = "";

            if (reservas.length == 0)
            {
                contenido = "No hay reservas para esta fecha.";
            }
            else
            {
                for (let i = 0; i < reservas.length; i++)
                {
                    contenido += "<p>Ocupado desde " + reservas[i].fechaHoraInicio +
                    " durante " + reservas[i].cantidadHoras + " horas.</p>";
                }
            }

            document.getElementById("ocupacionDelDia").innerHTML = contenido;

        });

});
