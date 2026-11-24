

// OCUPACION DEL DIA

let fechaOcupacion = document.getElementById("fechaOcupacion");

if (fechaOcupacion != null)
{
    fechaOcupacion.addEventListener("change", function () {

        let fecha = fechaOcupacion.value;
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
}


// VER MAS ESPACIOS

let botonVerMas = document.getElementById("botonVerMas");

if (botonVerMas != null)
{
    botonVerMas.addEventListener("click", function () {

        let ultimoId = document.getElementById("ultimoId").value;

        fetch("/Espacios/VerMas?ultimoId=" + ultimoId)
            .then(response => response.text())
            .then(data => {

                if (data.trim() == "")
                {
                    botonVerMas.style.display = "none";
                }
                else
                {
                    let nuevasTarjetas = document.createElement("div");
                    nuevasTarjetas.innerHTML = data;

                    let tarjetasNuevas = nuevasTarjetas.getElementsByClassName("tarjeta-espacio");

                    let cantidadTarjetas = tarjetasNuevas.length;

                    let ultimaTarjeta = tarjetasNuevas[cantidadTarjetas - 1];

                    ultimoId = ultimaTarjeta.getAttribute("data-id");

                    document.getElementById("contenedorEspacios").innerHTML += data;

                    document.getElementById("ultimoId").value = ultimoId;

                    if (cantidadTarjetas < 8)
                    {
                        botonVerMas.style.display = "none";
                    }
                }

            });

    });
}


// VALIDAR REGISTRO

function ValidarRegistro()
{
    let contrasena = document.getElementById("Contrasena").value;
    let repetirContrasena = document.getElementById("RepetirContrasena").value;

    if (contrasena != repetirContrasena)
    {
        alert("Las contraseñas no coinciden.");
        return false;
    }

    return true;
}

// CANCELAR RESERVAS

let botonesCancelar = document.getElementsByClassName("botonCancelar");

for (let i = 0; i < botonesCancelar.length; i++)
{
    botonesCancelar[i].addEventListener("click", function () {

        let idReserva = this.getAttribute("data-id");

        fetch("/Reservas/Cancelar", {
            method: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            body: "idReserva=" + idReserva
        })
        .then(response => response.json())
        .then(resultado => {

            if (resultado == true)
            {
                document.getElementById("reserva-" + idReserva).remove();
            }
            else
            {
                alert("No se pudo cancelar. Deben faltar al menos 24 horas para que comience la reserva.");
            }

        });

    });
}
