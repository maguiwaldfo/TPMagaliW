
let ultimoId = 8;

document.getElementById("botonVerMas").addEventListener("click", function () {

    fetch("/Espacios/VerMas?ultimoId=" + ultimoId)
        .then(response => response.text())
        .then(data => {

            if (data.trim() == "") {
                document.getElementById("botonVerMas").style.display = "none";
            }
            else {
                document.getElementById("contenedorEspacios").innerHTML += data;

                let tarjetas = document.getElementsByClassName("tarjeta-espacio");
                ultimoId = tarjetas[tarjetas.length - 1].querySelector("a").href.split("id=")[1];

                if (tarjetas.length < ultimoId) {
                    document.getElementById("botonVerMas").style.display = "none";
                }
            }

        });

});
