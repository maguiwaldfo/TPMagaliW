
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
