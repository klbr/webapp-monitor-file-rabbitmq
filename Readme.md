## Setup

### EF Core
- Update Migrations:
    - ```$ dotnet ef database update -s WebAppDesafio -p DataDesafio```

### RabbitMQ:
- Install Erlang from: [Erlang.org](https://www.erlang.org/downloads)
- Install RabbitMQ from: [RabbitMQ.com](https://www.rabbitmq.com/download.html)
- Enable Management (PowerShell):
    - ```C:\Program Files\RabbitMQ Server\rabbitmq_server-3.7.15\sbin> ./rabbitmq-plugins.bat enable rabbitmq_management```
    - http://localhost:15672
    - user: guest
    - password: guest

### SQLServer
 - Install Developer: [Microsoft.com/Sql-Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)


## Running

- Open solution with Visual Studio 2019

### Web Application: *Desafio.WebApp*
- Run on 5000/5001 ports
- https://localhost:5001 (UploadFile)
- https://localhost:5001/Report (Reports)
- https://localhost:5001/Report/Details?id=[id] (Report For File)
- Producer for Queue

### Background Application: *Desafio.Jobs*
- Producer/Consumer for queue
- Limit of 4 request/min to VirusTotal API
  - Each request within 15 seconds

### Console Application for Upload: *Desafio.ConsoleUpload*
- Receives list of file paths as argument and send to WebApi
