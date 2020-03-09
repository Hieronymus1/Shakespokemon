
## Instructions

```
docker build -t api-host .
docker run -d -p 8080:80 api-host
```

The service will listen on [http://localhost:8080](http://localhost:8080).