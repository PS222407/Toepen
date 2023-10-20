# Toepen

## Getting started
Just clone and run

### Deployment
In nginx config dont use keep-active, this breaks the websocket instead use this  
```bash
proxy_set_header   Connection $http_connection;
```
