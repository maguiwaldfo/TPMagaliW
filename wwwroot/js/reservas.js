
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
