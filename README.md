# Mezubo_API
Este proyecto representa una ruleta de apuestas online

**Estas instrucciones te permitirán obtener una copia del proyecto en funcionamiento en tu máquina local para propósitos de desarrollo y pruebas.**

**Pre-requisitos**

* .Net Core 5.0
* MongoDB 4.4
* Visual studio 2019

- Opcionales
    1. SourceTree (Cliente para manejo de git)

**Compilación**
1. Desrcargar o clonar el proyecto
2. Abra la solución en Visual Studio 2019 (**Preferiblemente**)
3. En la pestaña *Solution Explorer (Explorador de la solución)* haga click derecho sobre la solución y seleccione la opción *Clean (Limpiar)*
4. En la pestaña *Solution Explorer (Explorador de la solución)* haga click derecho sobre la solución y seleccione la opción *Build Solution (Compilar)*
5. Haga click en el botón Play(IIS Express) o oprima la tecla F5
6. Espere que se compile la solución y se abra la ventana de Swagger
7. Consulte los diferentes métodos

**Compilación Docker (Opcional)**
1. Desrcargar o clonar el proyecto
2. Abrir un símbolo del sistema o power shell y navegar hasta la carpeta donde se encuentra el proyecto.
3. Utilice los siguientes comandos para compilar y ejecutar la imagen de Docker:
 - docker build -t mezubo_api .
 - docker run -d -p 8080:80 --name Mezubo_Api mezubo_api
4. Dirigase a http://localhost:8080/ para acceder a su aplicación en un navegador web.
    * Para más información y/o detalle consulte https://docs.docker.com/engine/examples/dotnetcore/

**Construido con**

* .Net Core 5.0
* MongoDB

**Autores**

* Yoe Cardenas - Desarrollador full stack
