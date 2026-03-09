# 📦 Asisya Catalog API

Una API RESTful robusta, segura y escalable construida con **.NET 8** y **PostgreSQL** para la gestión eficiente de catálogos y productos, diseñada para reflejar habilidades técnicas y criterios arquitectónicos sólidos.

## 🏗️ 1. Arquitectura
El proyecto fue diseñado utilizando principios de **Clean Architecture** y diseño basado en capas para asegurar un bajo acoplamiento y alta cohesión:
* **Capas del Proyecto:** Dividido lógicamente en API (Presentación), Services (Casos de Uso), Data (Persistencia) y Entity (Modelos de Dominio).
* **Uso de DTOs:** No se exponen las entidades de base de datos directamente al cliente. Se implementó un mapeo explícito hacia Data Transfer Objects (DTOs).
* **Inyección de Dependencias:** Gestión centralizada del ciclo de vida de los servicios para facilitar el testing.

## 🚀 2. Escalabilidad y Performance en Cloud
Preparada para soportar altas cargas de trabajo y escalar horizontalmente:
* **Contenerización (ECS/EKS):** La API está dockerizada, lista para ser desplegada en orquestadores con Auto Scaling Groups.
* **Balanceo de Carga:** Operación detrás de un Application Load Balancer (ALB) para distribuir el tráfico.
* **Base de Datos (RDS):** Uso de bases de datos administradas con réplicas de lectura.
* **Procesamiento Masivo:** El endpoint de carga masiva (`POST /Product`) utiliza estrategias eficientes (Batch Inserts) para la inserción masiva de 100.000 productos. En escenarios de carga extrema, soporta la integración de colas (SQS/RabbitMQ) y procesamiento asíncrono.

## ⚙️ 3. Ejecución Local (Sin Docker)
**Prerrequisitos:** .NET 8 SDK, PostgreSQL y Git.

**Clonar y Configurar:**
`git clone <URL_DEL_REPOSITORIO>`
`cd Asisya`

Crea un archivo `.env` en la raíz con la siguiente configuración:
`ConnectionStrings__DefaultConnection=Host=localhost;Database=AsisyaDB;Username=tu_usuario;Password=tu_password`
`JWT_KEY=UnaClaveSecretaMuyLargaYSeguraParaProduccion123!`
`JWT_ISSUER=AsisyaApi`
`JWT_AUDIENCE=AsisyaClient`

**Compilar y Ejecutar:**
`dotnet build`
`cd Asisya.Api`
`dotnet ef database update`
`dotnet run`

## 🐳 4. Ejecución Local con Docker (Recomendada)
El proyecto incluye un `Dockerfile` funcional y `docker-compose.yml`.
1. Asegúrate de crear el archivo `.env` en la raíz (ver sección anterior).
2. Ejecuta: `docker-compose up --build -d`
3. La API estará disponible en `http://localhost:5000/swagger`.

## 🔐 5. Seguridad y Endpoints
Se implementó autenticación basada en **JWT** para asegurar endpoints críticos.
* **Auth:** `POST /api/Auth/register` y `POST /api/Auth/login`.
* **Negocio:**
  * `POST /Category/` → Crear categorías (Ej: SERVIDORES, CLOUD).
  * `POST /Product/` → Generar y guardar productos aleatorios (carga masiva).
  * `GET /Products/` → Listar productos con paginación, filtros y búsqueda.
  * `GET /Products/{id}` → Detalle de producto con foto de la categoría.
  * `PUT` / `DELETE` → Actualizar o borrar productos.

## 🧪 6. Pruebas Automatizadas
Suite de pruebas unitarias y de integración utilizando xUnit, FluentAssertions y Moq.
Ejecutar con: `dotnet test`

## 🔄 7. CI/CD y DevOps
Pipeline de **GitHub Actions** (`.github/workflows/dotnet-ci.yml`) que automatiza la verificación del proyecto (build y test) ante cada push/PR en las ramas principales. Código alojado en repositorio público.