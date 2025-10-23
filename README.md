# Toepen

## Getting started
- clone repository

### On Windows
after running open a reverse proxy for example:
```bash
iisexpress-proxy 5273 to 5555
```

### Deployment
In nginx config dont use keep-active, this breaks the websocket instead use this  
```bash
proxy_set_header   Connection $http_connection;
```
