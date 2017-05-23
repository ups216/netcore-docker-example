# Docker4Dev 8 Demo Script 

## TIPs

clean up all running containers

```
    FOR /f "tokens=*" %i IN ('docker ps -a -q') DO docker stop %i
    FOR /f "tokens=*" %i IN ('docker ps -a -q') DO docker rm %i
```
## 8.1 无中生有

```
	docker-compose -f docker-compose.ci.build.yml up 
```

## 8.2 借刀杀人

Run the following command to start up a Sql-server-linux container

Empty the folder C:\dataleixu\sources\repos\netcore-docker-example\netcoredocker.db

```
    docker run -e ACCEPT_EULA=Y -e SA_PASSWORD=P2ssw0rd -p 1433:1433 -d -v C:\dataleixu\sources\repos\netcore-docker-example\netcoredocker.db:/var/opt/mssql/ --name sql-dev microsoft/mssql-server-linux
```

Open SQL Management Studio and connect to localhost/sa/P2ssw0rd

Uncomment code for Entity Framework


## 8.3 五谷丰登

Explain the usage of -e to enable multiple container instance

Show the code of GetConfigure (deal with appsettings.json and environment variable formats)

update docker-compose.vs.debug.yml, highlight the difference in CONNECTIONSTRINGS_MSSQL and appsettings.json

```
version: '2'

services:
  netcoredocker.web:
    image: netcoredocker.web:dev
    build:
      args:
        source: ${DOCKER_BUILD_SOURCE}
    environment:
      - DOTNET_USE_POLLING_FILE_WATCHER=1
      - CONNECTIONSTRINGS_MSSQL=Server=tcp:netcoredocker.db,1433;Database=netcoredocker;User ID=sa;Password=P2ssw0rd;
    volumes:
      - ./netcoredocker.web:/app
      - ~/.nuget/packages:/root/.nuget/packages:ro
      - ~/clrdbg:/clrdbg:ro
    entrypoint: tail -f /dev/null
    labels:
      - "com.microsoft.visualstudio.targetoperatingsystem=linux"

  netcoredocker.db:
    image: microsoft/mssql-server-linux:ctp-2.0
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=P2ssw0rd
    volumes:
      - .\netcoredocker.db\:/var/opt/mssql/
    ports:
      - "1433:1433"


```

Press F5 to start debugging

run 

```
    docker logs -f {db container id}
```

## 8.4 万剑齐发

Open a command console 

```
    git clone https://github.com/ups216/docker-compose-demo.git
    cd docker-compose-demo
    docker-compose up 
```

Show the app in Browser

Open another command console

```
    cd docker-compose-demo
    docker-compose scale web=5
```

Refresh the app in browser

## 8.5 桃园结义

```
    docker-machine ssh lx-ea-ub1604dev01
    git clone https://github.com/ups216/docker-compose-demo.git
    cd docker-compose-demo
    docker-compose up
```

## 8.6 铁索连环

```
    git clone https://github.com/ups216/eShopOnContainers

```




